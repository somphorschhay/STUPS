﻿/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 4/1/2012
 * Time: 10:46 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace EsxiMgmt.Cmdlets.Commands
{
    using System.Management.Automation;
        
    /// <summary>
    /// Description of NewESXiFSDirectoryCommand.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "ESXiFSDirectory")]
    public class NewESXiFSDirectoryCommand : FileSystemCmdletBase
    {
        public NewESXiFSDirectoryCommand()
        {
        }
    }
}
