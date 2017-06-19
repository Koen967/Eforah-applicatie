using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EforahWebapp.Tests.Mocks
{
    public class HttpSessionMock : HttpSessionStateBase
    {
        private readonly Dictionary<string, object> objects = new Dictionary<string, object>();

        public override object this[string name]
        {
            get
            {
                object result = null;

                if (objects.ContainsKey(name))
                {
                    result = objects[name];
                }

                return result;

            }
            set
            {
                objects[name] = value;
            }
        }
    }
}
