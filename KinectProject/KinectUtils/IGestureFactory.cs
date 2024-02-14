using KicectClasse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectUtils
{
    public interface IGestureFactory
    {
        public IEnumerable<BaseGesture> CreateGestures();
    }
}
