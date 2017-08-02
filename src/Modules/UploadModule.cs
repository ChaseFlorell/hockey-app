using System.Collections.Generic;
using System.Threading.Tasks;
using HockeyApp.Http;
using HockeyApp.Models;
using Mono.Options;
using Resx = HockeyApp.Properties.Resources;

namespace HockeyApp.Modules
{
    public class UploadModule : Module
    {
        private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>();
        private string _appId;
        private string _hockeyAppToken;

        public UploadModule() : base(Resx.Upload_ModuleKeyword, Resx.Upload_ModuleDescription)
        {
            OptionSet = new OptionSet
            {
                // required
                {"token=", Resx.Upload_TokenDescription, v => _hockeyAppToken = v},
                {"appId=", Resx.Upload_AppIdDescription, v => _appId = v},
                {"ipa=", Resx.Upload_IpaDescription, v => _parameters["ipa"] = v},

                // optional
                {"dsym:", Resx.Upload_DSYMDescription, v => _parameters["dysm"] = v},
                {"notes:", Resx.Upload_NotesDescription, v => _parameters["notes"] = v},
                {"notesType:", Resx.Upload_NotesTypeDescription, v => _parameters["notes_type"] = v},
                {"notify:", Resx.Upload_NotifyDescription, v => _parameters["notify"] = v},
                {"status:", Resx.Upload_StatusDescription, v => _parameters["status"] = v},
                {"strategy:", Resx.Upload_StrategyDescription, v => _parameters["strategy"] = v},
                {"tags:", Resx.Upload_TagsDescription, v => _parameters["tags"] = v},
                {"teams:", Resx.Upload_TeamsDescription, v => _parameters["teams"] = v},
                {"users:", Resx.Upload_UsersDescription, v => _parameters["users"] = v},
                {"releaseType:", Resx.Upload_ReleaseTypeDescription, v => _parameters["release_type"] = v},

                // boolean
                {"mandatory|m", Resx.Upload_MandatoryDescription, v => _parameters["mandatory"] = v != null ? "1" : "0"}
            };
            AddDefaultOptions();
        }

        public override async Task<IResponseObject> Handle()
        {
            using (var client = new HockeyAppClient())
            {
                return await client.UploadAsync(_hockeyAppToken, _appId, _parameters);
            }
        }
    }
}