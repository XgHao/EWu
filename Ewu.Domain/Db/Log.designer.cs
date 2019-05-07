﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ewu.Domain.Db
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="LinJiaoFengJu")]
	public partial class LogDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    partial void InsertLogBrowse(LogBrowse instance);
    partial void UpdateLogBrowse(LogBrowse instance);
    partial void DeleteLogBrowse(LogBrowse instance);
    partial void InsertLogLogin(LogLogin instance);
    partial void UpdateLogLogin(LogLogin instance);
    partial void DeleteLogLogin(LogLogin instance);
    #endregion
		
		public LogDataContext() : 
				base(global::Ewu.Domain.Properties.Settings.Default.LinJiaoFengJuConnectionString1, mappingSource)
		{
			OnCreated();
		}
		
		public LogDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LogDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LogDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LogDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<LogBrowse> LogBrowse
		{
			get
			{
				return this.GetTable<LogBrowse>();
			}
		}
		
		public System.Data.Linq.Table<LogLogin> LogLogin
		{
			get
			{
				return this.GetTable<LogLogin>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.LogBrowse")]
	public partial class LogBrowse : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _BLogUID;
		
		private string _BrowserID;
		
		private string _TreasureID;
		
		private System.DateTime _BrowserTime;
		
		private string _BrowseIP;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnBLogUIDChanging(System.Guid value);
    partial void OnBLogUIDChanged();
    partial void OnBrowserIDChanging(string value);
    partial void OnBrowserIDChanged();
    partial void OnTreasureIDChanging(string value);
    partial void OnTreasureIDChanged();
    partial void OnBrowserTimeChanging(System.DateTime value);
    partial void OnBrowserTimeChanged();
    partial void OnBrowseIPChanging(string value);
    partial void OnBrowseIPChanged();
    #endregion
		
		public LogBrowse()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BLogUID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid BLogUID
		{
			get
			{
				return this._BLogUID;
			}
			set
			{
				if ((this._BLogUID != value))
				{
					this.OnBLogUIDChanging(value);
					this.SendPropertyChanging();
					this._BLogUID = value;
					this.SendPropertyChanged("BLogUID");
					this.OnBLogUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BrowserID", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string BrowserID
		{
			get
			{
				return this._BrowserID;
			}
			set
			{
				if ((this._BrowserID != value))
				{
					this.OnBrowserIDChanging(value);
					this.SendPropertyChanging();
					this._BrowserID = value;
					this.SendPropertyChanged("BrowserID");
					this.OnBrowserIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TreasureID", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string TreasureID
		{
			get
			{
				return this._TreasureID;
			}
			set
			{
				if ((this._TreasureID != value))
				{
					this.OnTreasureIDChanging(value);
					this.SendPropertyChanging();
					this._TreasureID = value;
					this.SendPropertyChanged("TreasureID");
					this.OnTreasureIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BrowserTime", DbType="DateTime NOT NULL")]
		public System.DateTime BrowserTime
		{
			get
			{
				return this._BrowserTime;
			}
			set
			{
				if ((this._BrowserTime != value))
				{
					this.OnBrowserTimeChanging(value);
					this.SendPropertyChanging();
					this._BrowserTime = value;
					this.SendPropertyChanged("BrowserTime");
					this.OnBrowserTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BrowseIP", DbType="NVarChar(50)")]
		public string BrowseIP
		{
			get
			{
				return this._BrowseIP;
			}
			set
			{
				if ((this._BrowseIP != value))
				{
					this.OnBrowseIPChanging(value);
					this.SendPropertyChanging();
					this._BrowseIP = value;
					this.SendPropertyChanged("BrowseIP");
					this.OnBrowseIPChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.LogLogin")]
	public partial class LogLogin : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _LoginUID;
		
		private string _LoginerID;
		
		private string _LoginIP;
		
		private System.DateTime _LoginTime;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnLoginUIDChanging(System.Guid value);
    partial void OnLoginUIDChanged();
    partial void OnLoginerIDChanging(string value);
    partial void OnLoginerIDChanged();
    partial void OnLoginIPChanging(string value);
    partial void OnLoginIPChanged();
    partial void OnLoginTimeChanging(System.DateTime value);
    partial void OnLoginTimeChanged();
    #endregion
		
		public LogLogin()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LoginUID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid LoginUID
		{
			get
			{
				return this._LoginUID;
			}
			set
			{
				if ((this._LoginUID != value))
				{
					this.OnLoginUIDChanging(value);
					this.SendPropertyChanging();
					this._LoginUID = value;
					this.SendPropertyChanged("LoginUID");
					this.OnLoginUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LoginerID", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string LoginerID
		{
			get
			{
				return this._LoginerID;
			}
			set
			{
				if ((this._LoginerID != value))
				{
					this.OnLoginerIDChanging(value);
					this.SendPropertyChanging();
					this._LoginerID = value;
					this.SendPropertyChanged("LoginerID");
					this.OnLoginerIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LoginIP", DbType="NVarChar(50)")]
		public string LoginIP
		{
			get
			{
				return this._LoginIP;
			}
			set
			{
				if ((this._LoginIP != value))
				{
					this.OnLoginIPChanging(value);
					this.SendPropertyChanging();
					this._LoginIP = value;
					this.SendPropertyChanged("LoginIP");
					this.OnLoginIPChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LoginTime", DbType="DateTime NOT NULL")]
		public System.DateTime LoginTime
		{
			get
			{
				return this._LoginTime;
			}
			set
			{
				if ((this._LoginTime != value))
				{
					this.OnLoginTimeChanging(value);
					this.SendPropertyChanging();
					this._LoginTime = value;
					this.SendPropertyChanged("LoginTime");
					this.OnLoginTimeChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
