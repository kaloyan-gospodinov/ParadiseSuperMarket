#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the ClassGenerator.ttinclude code generation file.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Data.Common;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;
using OpenAccess.Data;

namespace OpenAccess.Data	
{
	public partial class Product
	{
		private int _id;
		public virtual int Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}
		
		private string _productName;
		public virtual string ProductName
		{
			get
			{
				return this._productName;
			}
			set
			{
				this._productName = value;
			}
		}
		
		private double? _basePrice;
		public virtual double? BasePrice
		{
			get
			{
				return this._basePrice;
			}
			set
			{
				this._basePrice = value;
			}
		}
		
		private int _measure_id;
		public virtual int Measure_id
		{
			get
			{
				return this._measure_id;
			}
			set
			{
				this._measure_id = value;
			}
		}
		
		private int _vendor_id;
		public virtual int Vendor_id
		{
			get
			{
				return this._vendor_id;
			}
			set
			{
				this._vendor_id = value;
			}
		}
		
		private Measure _measure;
		public virtual Measure Measure
		{
			get
			{
				return this._measure;
			}
			set
			{
				this._measure = value;
			}
		}
		
		private Vendor _vendor;
		public virtual Vendor Vendor
		{
			get
			{
				return this._vendor;
			}
			set
			{
				this._vendor = value;
			}
		}
		
	}
}
#pragma warning restore 1591
