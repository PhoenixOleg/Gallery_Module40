using System;
using System.Collections.Generic;
using System.Text;

namespace Gallery_Module40.Models
{
    public class SourceFolder
    {
        public string folderName;
        public string sourceName;

        public SourceFolder(string FolderName, string SourceName) 
        { 
            folderName = FolderName;
            sourceName = SourceName;
        }
    }
}
