using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Task.DB;

namespace Task.Serialization
{
	class OrderDataContractSurrogate : IDataContractSurrogate
	{
		public Type GetDataContractType(Type type)
		{
			if (type == typeof(Employee))
				return typeof(Employee);
			else if (type == typeof(Shipper))
				return typeof(Shipper);
			else if (type == typeof(Customer))
				return typeof(Customer);
			else if (type == typeof(IEnumerable<Order_Detail>))
				return typeof(IEnumerable<Order_Detail>);
			else if (type == typeof(IEnumerable<Order>))
				return typeof(IEnumerable<Order>);
			else if (type == typeof(Order))
				return typeof(Order);
			return type;
		}

		public object GetObjectToSerialize(object obj, Type targetType)
		{
			if (targetType == typeof(Employee))
				return (Employee)obj;
			else if (targetType == typeof(Customer))
				return (Customer)obj;
			else if (targetType == typeof(Order_Detail))
				return (Order_Detail)obj;
			else if (targetType == typeof(Shipper))
				return (Shipper)obj;
			else if (targetType == typeof(Customer))
				return (Customer)obj;
			else if (targetType == typeof(IEnumerable<Order_Detail>))
				return (IEnumerable<Order_Detail>)obj;
			else if (targetType == typeof(IEnumerable<Order>))
				return (IEnumerable<Order>)obj;
			else if (targetType == typeof(Order))
				return (Order)obj;
			else if (targetType == typeof(Category))
				return (Category)obj;
			return obj;
		}

		public object GetDeserializedObject(object obj, Type targetType)
		{
			return obj;
		}

		public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
		{
			throw new NotImplementedException();
		}

		public object GetCustomDataToExport(Type clrType, Type dataContractType)
		{
			throw new NotImplementedException();
		}

		public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
		{
			throw new NotImplementedException();
		}

		public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
		{
			throw new NotImplementedException();
		}

		public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
		{
			throw new NotImplementedException();
		}
	}
}
