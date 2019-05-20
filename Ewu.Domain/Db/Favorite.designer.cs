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
	public partial class FavoriteDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    partial void InsertFavorite(Favorite instance);
    partial void UpdateFavorite(Favorite instance);
    partial void DeleteFavorite(Favorite instance);
    #endregion
		
		public FavoriteDataContext() : 
				base(global::Ewu.Domain.Properties.Settings.Default.LinJiaoFengJuConnectionString2, mappingSource)
		{
			OnCreated();
		}
		
		public FavoriteDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public FavoriteDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public FavoriteDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public FavoriteDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Favorite> Favorite
		{
			get
			{
				return this.GetTable<Favorite>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Favorite")]
	public partial class Favorite : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _FavoriteUID;
		
		private string _UserID;
		
		private string _TreasureID;
		
		private System.Nullable<System.DateTime> _FavoriteTime;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnFavoriteUIDChanging(string value);
    partial void OnFavoriteUIDChanged();
    partial void OnUserIDChanging(string value);
    partial void OnUserIDChanged();
    partial void OnTreasureIDChanging(string value);
    partial void OnTreasureIDChanged();
    partial void OnFavoriteTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnFavoriteTimeChanged();
    #endregion
		
		public Favorite()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FavoriteUID", DbType="VarChar(50) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string FavoriteUID
		{
			get
			{
				return this._FavoriteUID;
			}
			set
			{
				if ((this._FavoriteUID != value))
				{
					this.OnFavoriteUIDChanging(value);
					this.SendPropertyChanging();
					this._FavoriteUID = value;
					this.SendPropertyChanged("FavoriteUID");
					this.OnFavoriteUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserID", DbType="VarChar(50)")]
		public string UserID
		{
			get
			{
				return this._UserID;
			}
			set
			{
				if ((this._UserID != value))
				{
					this.OnUserIDChanging(value);
					this.SendPropertyChanging();
					this._UserID = value;
					this.SendPropertyChanged("UserID");
					this.OnUserIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TreasureID", DbType="VarChar(50)")]
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FavoriteTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> FavoriteTime
		{
			get
			{
				return this._FavoriteTime;
			}
			set
			{
				if ((this._FavoriteTime != value))
				{
					this.OnFavoriteTimeChanging(value);
					this.SendPropertyChanging();
					this._FavoriteTime = value;
					this.SendPropertyChanged("FavoriteTime");
					this.OnFavoriteTimeChanged();
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
