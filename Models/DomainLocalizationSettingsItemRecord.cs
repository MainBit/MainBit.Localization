using Orchard.Localization.Records;

namespace MainBit.Localization.Models
{
    public class DomainLocalizationSettingsItemRecord
    {
        public virtual int Id { get; set; }
        public virtual int DomainLocalizationSettingsPartRecord_Id { get; set; }
        public virtual string Culture { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual int Position { get; set; }
        public virtual string Domain { get; set; }
    }
}