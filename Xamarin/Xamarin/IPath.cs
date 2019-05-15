using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin
{
    public interface IPath
    {
        string GetDatabasePath(string filename);
    }
}
