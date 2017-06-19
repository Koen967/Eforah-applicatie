using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Eforah_BetaalApp.Droid.Test.Mocks
{
    class AttributeSetMock : Android.Util.IAttributeSet
    {
        public int AttributeCount => 0;

        public string ClassAttribute => throw new NotImplementedException();

        public string IdAttribute => throw new NotImplementedException();

        public string PositionDescription => throw new NotImplementedException();

        public int StyleAttribute => throw new NotImplementedException();

        public IntPtr Handle => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool GetAttributeBooleanValue(int index, bool defaultValue)
        {
            throw new NotImplementedException();
        }

        public bool GetAttributeBooleanValue(string @namespace, string attribute, bool defaultValue)
        {
            throw new NotImplementedException();
        }

        public float GetAttributeFloatValue(int index, float defaultValue)
        {
            throw new NotImplementedException();
        }

        public float GetAttributeFloatValue(string @namespace, string attribute, float defaultValue)
        {
            throw new NotImplementedException();
        }

        public int GetAttributeIntValue(int index, int defaultValue)
        {
            throw new NotImplementedException();
        }

        public int GetAttributeIntValue(string @namespace, string attribute, int defaultValue)
        {
            throw new NotImplementedException();
        }

        public int GetAttributeListValue(int index, string[] options, int defaultValue)
        {
            throw new NotImplementedException();
        }

        public int GetAttributeListValue(string @namespace, string attribute, string[] options, int defaultValue)
        {
            throw new NotImplementedException();
        }

        public string GetAttributeName(int index)
        {
            throw new NotImplementedException();
        }

        public int GetAttributeNameResource(int index)
        {
            throw new NotImplementedException();
        }

        public int GetAttributeResourceValue(int index, int defaultValue)
        {
            throw new NotImplementedException();
        }

        public int GetAttributeResourceValue(string @namespace, string attribute, int defaultValue)
        {
            throw new NotImplementedException();
        }

        public int GetAttributeUnsignedIntValue(int index, int defaultValue)
        {
            throw new NotImplementedException();
        }

        public int GetAttributeUnsignedIntValue(string @namespace, string attribute, int defaultValue)
        {
            throw new NotImplementedException();
        }

        public string GetAttributeValue(int index)
        {
            throw new NotImplementedException();
        }

        public string GetAttributeValue(string @namespace, string name)
        {
            throw new NotImplementedException();
        }

        public int GetIdAttributeResourceValue(int defaultValue)
        {
            throw new NotImplementedException();
        }
    }
}