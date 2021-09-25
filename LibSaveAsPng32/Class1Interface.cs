using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LibSaveAsPng32
{
    [ComVisible(true)]
    [Guid("9fa9a9f0-fd5d-45eb-994e-2de7f71a019d")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface Class1Interface
    {
        void SavePictureAs(object picture, string saveTo);
    }
}
