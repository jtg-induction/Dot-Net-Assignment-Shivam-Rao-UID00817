using System;
using System.Reflection;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}