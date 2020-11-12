

using Common.Domain.BuildingBlocks;

namespace Support.Domain.ApplicationInfos
{
    public class ApplicationInfo : AggregateRoot<ApplicationInfoId>
    {
        public ApplicationInfo(NameApp nameApp, CodeApp codeApp, VersionApp versionApp) : base()
        {
            NameApp = nameApp;
            CodeApp = codeApp;
            VersionApp = versionApp;
        }

        public NameApp NameApp { get; set; }

        public VersionApp VersionApp { get; set; }

        public CodeApp CodeApp { get; set; }
    }
}
