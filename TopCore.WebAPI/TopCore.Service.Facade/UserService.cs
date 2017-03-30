﻿#region	License
//------------------------------------------------------------------------------------------------
// <Auto-generated>
//     <Author> Top Nguyen (http://topnguyen.net) </Author>
//     <Project> TopCore.Service.Facade </Project>
//     <File> 
//         <Name> UserService.cs </Name>
//         <Created> 29 03 2017 11:55:30 PM </Created>
//         <Key> 6B0A8F26-EBEE-4D43-8F5B-6A851550F37D </Key>
//     </File>
//     <Summary>
//         UserService
//     </Summary>
// </Auto-generated>
//------------------------------------------------------------------------------------------------
#endregion License

using TopCore.Framework.DependencyInjection.Attributes;

namespace TopCore.Service.Facade
{
    [PerRequestDependency(ServiceType = typeof(IUserService))]
    public class UserService : IUserService
    {
        public string GetUserName()
        {
            return "Top Nguyen Hard-code";
        }
    }
}