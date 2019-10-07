/*
*
* Copyright (c) Microsoft Corporation.
* All rights reserved.
*
* This code is licensed under the MIT License.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files(the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions :
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*
*/

using System;
using System.Threading.Tasks;
using Microsoft.InformationProtection;
using Microsoft.InformationProtection.File;
using Microsoft.InformationProtection.Protection;
using Microsoft.InformationProtection.Policy;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace MipSdkDotNetQuickstart
{
    class Program
    {
        private static readonly string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static readonly string appName = ConfigurationManager.AppSettings["app:Name"];
        private static readonly string appVersion = ConfigurationManager.AppSettings["app:Version"];

        static void Main(string[] args)
        {

            // Create ApplicationInfo, setting the clientID from Azure AD App Registration as the ApplicationId
            // If any of these values are not set API throws BadInputException.
            ApplicationInfo appInfo = new ApplicationInfo()
            {
                // ApplicationId should ideally be set to the same ClientId found in the Azure AD App Registration.
                // This ensures that the clientID in AAD matches the AppId reported in AIP Analytics.
                ApplicationId = clientId,
                ApplicationName = appName,
                ApplicationVersion = appVersion
            };

            // Initialize Action class, passing in AppInfo.
            Action action = new Action(appInfo);

            // To Do: List Templates when Protection API is available
            //IEnumerable<Label> labels = action.ListLabels();
                                                              
            // Prompt for path inputs
            Console.Write("Enter an input file path: ");
            string inputFilePath = Console.ReadLine();

            Console.Write("Enter an output file path: ");
            string outputFilePath = Console.ReadLine();


            var result = MenuOptions.ShowRightsRolesMenu();
            
            

            // Set file options from FileOptions struct. Used to set various parameters for FileHandler
            Action.FileOptions options = new Action.FileOptions
            {
                FileName = inputFilePath,
                OutputName = outputFilePath,             
                GenerateChangeAuditEvent = false,
                IsAuditDiscoveryEnabled = false                
            };

            if (result == MenuOptions.DescriptorMethod.Template)
            {
                Console.WriteLine("Enter a template ID: ");
                options.TemplatelId = Console.ReadLine();
            }

            else
            {
                options.UserRoles = MenuOptions.BuildUserRoles();
            }

            //Set the label on the file handler object
            Console.WriteLine(string.Format("Setting protection on {0}", inputFilePath));

            // Set label, commit change to outputfile, and send audit event if enabled.
            var fileResult = action.SetProtection(options);

            
            // Create a new handler to read the labeled file metadata.           
            Console.WriteLine(string.Format("Getting the protection details committed to file: {0}", outputFilePath));

            // Update options to read the previously generated file output.
            options.FileName = options.OutputName;

            // Read label from the previously labeled file.
            var protectionHandler = action.GetProtectionHandler(options);

            if (protectionHandler != null)
            {                
                Console.WriteLine("Owner: {0}", protectionHandler.Owner);
                Console.WriteLine("IssuedTo: {0}", protectionHandler.IssuedTo);
                foreach(var right in protectionHandler.Rights)
                {
                    Console.WriteLine("Right: {0}", right);
                }             
            }
            Console.WriteLine("Press a key to quit.");
            Console.ReadKey();
        }
    }
}
