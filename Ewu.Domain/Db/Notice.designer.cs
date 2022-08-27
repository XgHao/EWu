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
	public partial class NoticeDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    partial void InsertNotice(Notice instance);
    partial void UpdateNotice(Notice instance);
    partial void DeleteNotice(Notice instance);
    #endregion
		
		public NoticeDataContext() : 
				base(global::Ewu.Domain.Properties.Settings.Default.LinJiaoFengJuConnectionString1, mappingSource)
		{
			OnCreated();
		}
		
		public NoticeDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public NoticeDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public NoticeDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public NoticeDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Notice> Notice
		{
			get
			{
				return this.GetTable<Notice>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Notice")]
	public partial class Notice : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _NoticeUID;
		
		private string _RecipientID;
		
		private string _SponsorID;
		
		private string _NoticeObject;
		
		private string _NoticeContent;
		
		private System.DateTime _NoticeTime;
		
		private bool _IsRead;
		
		private string _TreasureUID;
		
		private string _RelpyNoticeUID;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnNoticeUIDChanging(System.Guid value);
    partial void OnNoticeUIDChanged();
    partial void OnRecipientIDChanging(string value);
    partial void OnRecipientIDChanged();
    partial void OnSponsorIDChanging(string value);
    partial void OnSponsorIDChanged();
    partial void OnNoticeObjectChanging(string value);
    partial void OnNoticeObjectChanged();
    partial void OnNoticeContentChanging(string value);
    partial void OnNoticeContentChanged();
    partial void OnNoticeTimeChanging(System.DateTime value);
    partial void OnNoticeTimeChanged();
    partial void OnIsReadChanging(bool value);
    partial void OnIsReadChanged();
    partial void OnTreasureUIDChanging(string value);
    partial void OnTreasureUIDChanged();
    partial void OnRelpyNoticeUIDChanging(string value);
    partial void OnRelpyNoticeUIDChanged();
    #endregion
		
		public Notice()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NoticeUID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid NoticeUID
		{
			get
			{
				return this._NoticeUID;
			}
			set
			{
				if ((this._NoticeUID != value))
				{
					this.OnNoticeUIDChanging(value);
					this.SendPropertyChanging();
					this._NoticeUID = value;
					this.SendPropertyChanged("NoticeUID");
					this.OnNoticeUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RecipientID", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string RecipientID
		{
			get
			{
				return this._RecipientID;
			}
			set
			{
				if ((this._RecipientID != value))
				{
					this.OnRecipientIDChanging(value);
					this.SendPropertyChanging();
					this._RecipientID = value;
					this.SendPropertyChanged("RecipientID");
					this.OnRecipientIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SponsorID", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string SponsorID
		{
			get
			{
				return this._SponsorID;
			}
			set
			{
				if ((this._SponsorID != value))
				{
					this.OnSponsorIDChanging(value);
					this.SendPropertyChanging();
					this._SponsorID = value;
					this.SendPropertyChanged("SponsorID");
					this.OnSponsorIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NoticeObject", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string NoticeObject
		{
			get
			{
				return this._NoticeObject;
			}
			set
			{
				if ((this._NoticeObject != value))
				{
					this.OnNoticeObjectChanging(value);
					this.SendPropertyChanging();
					this._NoticeObject = value;
					this.SendPropertyChanged("NoticeObject");
					this.OnNoticeObjectChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NoticeContent", DbType="NVarChar(MAX)")]
		public string NoticeContent
		{
			get
			{
				return this._NoticeContent;
			}
			set
			{
				if ((this._NoticeContent != value))
				{
					this.OnNoticeContentChanging(value);
					this.SendPropertyChanging();
					this._NoticeContent = value;
					this.SendPropertyChanged("NoticeContent");
					this.OnNoticeContentChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NoticeTime", DbType="DateTime NOT NULL")]
		public System.DateTime NoticeTime
		{
			get
			{
				return this._NoticeTime;
			}
			set
			{
				if ((this._NoticeTime != value))
				{
					this.OnNoticeTimeChanging(value);
					this.SendPropertyChanging();
					this._NoticeTime = value;
					this.SendPropertyChanged("NoticeTime");
					this.OnNoticeTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsRead", DbType="Bit NOT NULL")]
		public bool IsRead
		{
			get
			{
				return this._IsRead;
			}
			set
			{
				if ((this._IsRead != value))
				{
					this.OnIsReadChanging(value);
					this.SendPropertyChanging();
					this._IsRead = value;
					this.SendPropertyChanged("IsRead");
					this.OnIsReadChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TreasureUID", DbType="VarChar(50)")]
		public string TreasureUID
		{
			get
			{
				return this._TreasureUID;
			}
			set
			{
				if ((this._TreasureUID != value))
				{
					this.OnTreasureUIDChanging(value);
					this.SendPropertyChanging();
					this._TreasureUID = value;
					this.SendPropertyChanged("TreasureUID");
					this.OnTreasureUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RelpyNoticeUID", DbType="VarChar(50)")]
		public string RelpyNoticeUID
		{
			get
			{
				return this._RelpyNoticeUID;
			}
			set
			{
				if ((this._RelpyNoticeUID != value))
				{
					this.OnRelpyNoticeUIDChanging(value);
					this.SendPropertyChanging();
					this._RelpyNoticeUID = value;
					this.SendPropertyChanged("RelpyNoticeUID");
					this.OnRelpyNoticeUIDChanged();
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