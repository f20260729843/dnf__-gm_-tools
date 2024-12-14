using AY.DNF.GMTool.Db.Models;
using AY.DNF.GMTool.Db.Services;
using AY.DNF.GMTool.Online.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AY.DNF.GMTool.Online.ViewModels
{
    internal class OnlineMngPageViewModel : BindableBase
    {
        List<JobTreeModel> _jobTree;

        #region 属性

        private string _accountName;
        /// <summary>
        /// 账号
        /// </summary>
        public string AccountName
        {
            get { return _accountName; }
            set { SetProperty(ref _accountName, value); }
        }

        private string _characName;
        /// <summary>
        /// 角色名
        /// </summary>
        public string CharacName
        {
            get { return _characName; }
            set { SetProperty(ref _characName, value); }
        }

        private bool _isOnLine;
        /// <summary>
        /// 是否只看在线
        /// </summary>
        public bool IsOnLine
        {
            get { return _isOnLine; }
            set { SetProperty(ref _isOnLine, value); }
        }

        private ObservableCollection<OnlineMemberModel> _onlineList;

        public ObservableCollection<OnlineMemberModel> OnlineList
        {
            get { return _onlineList; }
            set { SetProperty(ref _onlineList, value); }
        }

        #endregion

        #region 事件

        ICommand? _searchCommand;
        /// <summary>
        /// 查询
        /// </summary>
        public ICommand SearchCommand => _searchCommand ??= new DelegateCommand(DoSearchCommand);

        #endregion

        public OnlineMngPageViewModel()
        {
            _onlineList = new ObservableCollection<OnlineMemberModel>();

            LoadJobs();
        }

        /// <summary>
        /// 完整职业树读取
        /// </summary>
        async void LoadJobs()
        {
            var jobs = await new GMToolService().GetJobs();
            _jobTree = jobs.Where(t => t.ParentId == "root").Select(t => new JobTreeModel
            {
                Id = t.Id,
                JobId = t.JobId,
                JobName = t.JobName,
                GrowJobs = t.GrowJobs!.Select(x => new JobTreeModel
                {
                    Id = x.Id,
                    JobName = x.JobName,
                    JobId = x.JobId,
                    GrowJobs = x.GrowJobs?.Select(y => new JobTreeModel
                    {
                        Id = y.Id,
                        JobName = y.JobName,
                        JobId = y.JobId,
                    }).ToList()!
                }).ToList()
            }).ToList();
        }

        /// <summary>
        /// 根据职业信息获取详细职业描述
        /// </summary>
        void JobInfoDetail(int baseJob, long growJob, out string jobName, out long jxCount, out string jxNames)
        {
            jobName = string.Empty;
            jxNames = string.Empty;

            var job = _jobTree.First(t => t.JobId == baseJob);
            jxCount = growJob / 16;
            var jx = growJob % 16;
            var jxJob = job.GrowJobs.FirstOrDefault(t => t.JobId == jx);

            if (jxJob == null)// 兼容性
            {
                jxNames = "未觉醒";
            }
            else
            {
                if (jxCount == 0)
                    jxNames = "未觉醒";
                else
                {
                    if (jxJob.GrowJobs == null || jxJob.GrowJobs.Count <= 0)
                        jxNames = jxJob.JobName;
                    else
                    {
                        var jc = jxCount;
                        var tmp = jxJob.GrowJobs.Where(t => t.JobId <= jc).ToList();
                        jxNames = tmp.Aggregate(string.Empty, (x, y) => x += $"{y.JobName} -> ").TrimEnd('-', '>', ' ');
                    }
                }
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        async void DoSearchCommand()
        {
            OnlineList.Clear();

            var infos = await new MemberService().OnlineMember(AccountName, CharacName, IsOnLine);

            infos.ForEach(t =>
            {
                JobInfoDetail(t.Job, t.GrowType, out var jn, out var jc, out var jxn);

                t.JobName = jn;
                t.GrowTypeNames = jxn;
            });

            OnlineList.AddRange(infos);
        }
    }
}
