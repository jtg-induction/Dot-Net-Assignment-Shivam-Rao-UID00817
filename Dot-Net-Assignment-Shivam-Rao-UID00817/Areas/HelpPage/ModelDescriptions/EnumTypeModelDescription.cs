using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Areas.HelpPage.ModelDescriptions
{
    public class EnumTypeModelDescription : ModelDescription
    {
        public EnumTypeModelDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Values { get; private set; }
    }
}