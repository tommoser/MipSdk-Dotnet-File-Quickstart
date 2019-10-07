using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.InformationProtection;
using Microsoft.InformationProtection.Protection;

namespace MipSdkDotNetQuickstart
{
    public static class MenuOptions
    {        
        public static List<UserRoles> BuildUserRoles()
        {
            List<UserRoles> userRoles = new List<UserRoles>();
            List<string> users = new List<string>();
            List<string> roles = new List<string>();
            bool addAnother = true;

            while (addAnother)
            {
                Console.WriteLine("Select a role: ");
                Console.WriteLine("0: Author");
                Console.WriteLine("1: Co-Owner ");
                Console.WriteLine("2: Reviewer");
                Console.WriteLine("3: Viewer");
                Console.Write("Selection: ");
                var selection = Console.ReadKey().KeyChar.ToString();

                switch (selection)
                {
                    case "0":
                        roles.Add(Roles.Author);
                        break;

                    case "1":
                        roles.Add(Roles.CoOwner);
                        break;

                    case "2":
                        roles.Add(Roles.Reviewer);
                        break;

                    case "3":
                        roles.Add(Roles.Viewer);
                        break;

                    default:
                        roles.Add(Roles.Viewer);
                        break;
                }


                string input = ".";
                while (input != string.Empty)
                {
                    Console.Write("\n\nEnter a user or group for this role. Enter a blank line to finish: ");
                    input = Console.ReadLine();

                    // NO validation on input here. Add it on your own if required. It's just a sample...
                    if (input != string.Empty)
                    {
                        users.Add(input);
                    }
                }

                userRoles.Add(new UserRoles(users, roles));
                Console.Write("\n\nAdd another? [y/N]");
                if(Console.ReadKey().KeyChar.ToString().ToLower() =="y")
                {
                    addAnother = true;
                }
                else
                {
                    addAnother = false;
                }
            }

            return userRoles;
        }

        public static DescriptorMethod ShowRightsRolesMenu()
        {
            Console.Write("\nDefine rights by roles? [y/n]: ");
            var decision = Console.ReadKey().KeyChar.ToString().ToLower();
            if(decision == "y")
            {
                return DescriptorMethod.Roles;
            }
           
            Console.WriteLine("Using template.");

            return DescriptorMethod.Template;
        }

        public enum DescriptorMethod
        {
            Rights = 0,
            Roles = 1,
            Template = 2
        }
    }
}
