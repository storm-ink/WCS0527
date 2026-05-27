using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Aga.Controls.Tree.NodeControls
{
	public abstract class BindableControl: NodeControl
	{
		private string _propertyName = "";
		[DefaultValue("")]
		public string DataPropertyName
		{
			get { return _propertyName; }
			set 
			{
				if (_propertyName == null)
					_propertyName = string.Empty;
				_propertyName = value; 
			}
		}

		public object GetValue(TreeNodeAdv node)
		{
            return _getValue(node, DataPropertyName);

            //PropertyInfo pi = GetPropertyInfo(node);
            //if (pi != null && pi.CanRead)
            //    return pi.GetValue(node.Tag, null);
            //else
            //    return null;
		}

		public void SetValue(TreeNodeAdv node, object value)
		{
			PropertyInfo pi = GetPropertyInfo(node);
			if (pi != null && pi.CanWrite)
			{
				try
				{
					pi.SetValue(node.Tag, value, null);
				}
				catch (TargetInvocationException ex)
				{
					if (ex.InnerException != null)
						throw new ArgumentException(ex.InnerException.Message, ex.InnerException);
					else
						throw new ArgumentException(ex.Message);
				}
			}
		}

		public Type GetPropertyType(TreeNodeAdv node)
		{
			if (node.Tag != null && !string.IsNullOrEmpty(DataPropertyName))
			{
				Type type = node.Tag.GetType();
				PropertyInfo pi = type.GetProperty(DataPropertyName);
				if (pi != null)
					return pi.PropertyType;
			}
			return null;
		}

        private object _getValue(TreeNodeAdv node,string dataPropertyName)
        {
            object varObject = node.Tag;
            Type type = varObject.GetType();
            foreach (var item in dataPropertyName.Split('.'))
            {
                if (item.EndsWith("()"))
                {
                    MethodInfo mi = type.GetMethod(item.Replace("()", ""));
                    if (mi == null)
                    {
                        return null;
                    }

                    varObject = mi.Invoke(varObject, null);
                    if (varObject == null)
                    {
                        return null;
                    }

                    type = varObject.GetType();
                }
                else
                {
                    PropertyInfo pi = type.GetProperty(item);
                    if (pi == null)
                    {
                        return null;
                    }
                    else
                    {
                        varObject = pi.GetValue(varObject, null);
                        if (varObject == null)
                        {
                            return null;
                        }

                        type = pi.PropertyType;
                    }
                }
            }

            return varObject;
        }

		private PropertyInfo GetPropertyInfo(TreeNodeAdv node)
		{
			if (node.Tag != null && !string.IsNullOrEmpty(DataPropertyName))
			{
				Type type = node.Tag.GetType();
				return type.GetProperty(DataPropertyName);
			}
			return null;
		}

		public override string ToString()
		{
			if (string.IsNullOrEmpty(DataPropertyName))
				return GetType().Name;
			else
				return string.Format("{0} ({1})", GetType().Name, DataPropertyName);
		}
	}
}
