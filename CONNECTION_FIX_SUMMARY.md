# 数据库连接立即断开问题修复

## 问题分析

经过排查，发现连接按钮功能中存在以下问题导致连接后立即断开：

### 1. 主要问题
- **连接成功后立即调用PingServer()**：在`DoConnectCommand()`方法中，连接成功后立即调用`PingServer()`方法
- **PingServer()自动断开逻辑**：如果ping失败，会自动调用`DoDisconnectCommand()`断开连接
- **时机问题**：连接刚建立时，Server变量可能还没有完全更新，导致ping失败

### 2. 次要问题
- **Ping任务中的变量错误**：使用了错误的取消令牌变量`ct`而不是`pct`
- **断开连接时未停止ping任务**：断开连接时没有正确停止ping任务的取消令牌

## 修复内容

### 1. 移除立即ping调用
```csharp
// 修复前
Growl.Success("连接成功");
OpenEnabled = false;
ConnectedForEnabled = true;
WriteCfg();
PingServer(); // 这行导致立即断开

// 修复后
Growl.Success("连接成功");
OpenEnabled = false;
ConnectedForEnabled = true;
WriteCfg();
// 连接成功后不立即ping，避免立即断开连接
// PingServer();
```

### 2. 修复ping任务中的变量错误
```csharp
// 修复前
if (ct.IsCancellationRequested) return;

// 修复后
if (pct.IsCancellationRequested) return;
```

### 3. 完善断开连接逻辑
```csharp
// 修复前
void DoDisconnectCommand()
{
    DbFrameworkScope.Disconnect();
    OpenEnabled = true;
    ConnectedForEnabled = false;
    _timeTaskCancelTokenSource.Cancel();
}

// 修复后
void DoDisconnectCommand()
{
    DbFrameworkScope.Disconnect();
    OpenEnabled = true;
    ConnectedForEnabled = false;
    _timeTaskCancelTokenSource.Cancel();
    _pingTaskCancelTokenSource.Cancel(); // 添加这行
}
```

## 修复效果

1. **连接稳定性**：连接成功后不会立即断开
2. **任务管理**：ping任务使用正确的取消令牌
3. **资源清理**：断开连接时正确停止所有后台任务

## 测试建议

1. 启动程序
2. 输入数据库连接信息
3. 点击连接按钮
4. 验证连接成功后不会立即断开
5. 验证ping状态正常显示

## 注意事项

- ping任务仍然会每分钟执行一次，用于监控服务器状态
- 如果服务器真的离线，ping失败后仍会自动断开连接（这是正确的行为）
- 修复只解决了连接建立时的立即断开问题，不影响正常的连接监控功能
