using System;
using System.IO;
using Xamarin.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDbPath))]
namespace Xamarin.Droid
{
    public class AndroidDbPath : IPath
    {
        public string GetDatabasePath(string filename)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
        }
    }
}