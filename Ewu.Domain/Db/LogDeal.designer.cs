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
	public partial class LogDealDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    partial void InsertLogDeal(LogDeal instance);
    partial void UpdateLogDeal(LogDeal instance);
    partial void DeleteLogDeal(LogDeal instance);
    #endregion
		
		public LogDealDataContext() : 
				base(global::Ewu.Domain.Properties.Settings.Default.LinJiaoFengJuConnectionString2, mappingSource)
		{
			OnCreated();
		}
		
		public LogDealDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LogDealDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LogDealDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LogDealDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<LogDeal> LogDeal
		{
			get
			{
				return this.GetTable<LogDeal>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.LogDeal")]
	public partial class LogDeal : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _DLogUID;
		
		private string _TraderSponsorID;
		
		private string _TreasureSponsorID;
		
		private string _TraderRecipientID;
		
		private string _TreasureRecipientID;
		
		private System.DateTime _DealBeginTime;
		
		private System.Nullable<System.DateTime> _DealEndTime;
		
		private string _DealStatus;
		
		private string _RemarkSToR;
		
		private string _RemarkRToS;
		
		private string _ScoreSToR;
		
		private string _ScoreRToS;
		
		private string _DeliveryAddressSponsorID;
		
		private string _DeliveryAddressRecipientID;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDLogUIDChanging(System.Guid value);
    partial void OnDLogUIDChanged();
    partial void OnTraderSponsorIDChanging(string value);
    partial void OnTraderSponsorIDChanged();
    partial void OnTreasureSponsorIDChanging(string value);
    partial void OnTreasureSponsorIDChanged();
    partial void OnTraderRecipientIDChanging(string value);
    partial void OnTraderRecipientIDChanged();
    partial void OnTreasureRecipientIDChanging(string value);
    partial void OnTreasureRecipientIDChanged();
    partial void OnDealBeginTimeChanging(System.DateTime value);
    partial void OnDealBeginTimeChanged();
    partial void OnDealEndTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnDealEndTimeChanged();
    partial void OnDealStatusChanging(string value);
    partial void OnDealStatusChanged();
    partial void OnRemarkSToRChanging(string value);
    partial void OnRemarkSToRChanged();
    partial void OnRemarkRToSChanging(string value);
    partial void OnRemarkRToSChanged();
    partial void OnScoreSToRChanging(string value);
    partial void OnScoreSToRChanged();
    partial void OnScoreRToSChanging(string value);
    partial void OnScoreRToSChanged();
    partial void OnDeliveryAddressSponsorIDChanging(string value);
    partial void OnDeliveryAddressSponsorIDChanged();
    partial void OnDeliveryAddressRecipientIDChanging(string value);
    partial void OnDeliveryAddressRecipientIDChanged();
    #endregion
		
		public LogDeal()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DLogUID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid DLogUID
		{
			get
			{
				return this._DLogUID;
			}
			set
			{
				if ((this._DLogUID != value))
				{
					this.OnDLogUIDChanging(value);
					this.SendPropertyChanging();
					this._DLogUID = value;
					this.SendPropertyChanged("DLogUID");
					this.OnDLogUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TraderSponsorID", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string TraderSponsorID
		{
			get
			{
				return this._TraderSponsorID;
			}
			set
			{
				if ((this._TraderSponsorID != value))
				{
					this.OnTraderSponsorIDChanging(value);
					this.SendPropertyChanging();
					this._TraderSponsorID = value;
					this.SendPropertyChanged("TraderSponsorID");
					this.OnTraderSponsorIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TreasureSponsorID", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string TreasureSponsorID
		{
			get
			{
				return this._TreasureSponsorID;
			}
			set
			{
				if ((this._TreasureSponsorID != value))
				{
					this.OnTreasureSponsorIDChanging(value);
					this.SendPropertyChanging();
					this._TreasureSponsorID = value;
					this.SendPropertyChanged("TreasureSponsorID");
					this.OnTreasureSponsorIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TraderRecipientID", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string TraderRecipientID
		{
			get
			{
				return this._TraderRecipientID;
			}
			set
			{
				if ((this._TraderRecipientID != value))
				{
					this.OnTraderRecipientIDChanging(value);
					this.SendPropertyChanging();
					this._TraderRecipientID = value;
					this.SendPropertyChanged("TraderRecipientID");
					this.OnTraderRecipientIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TreasureRecipientID", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string TreasureRecipientID
		{
			get
			{
				return this._TreasureRecipientID;
			}
			set
			{
				if ((this._TreasureRecipientID != value))
				{
					this.OnTreasureRecipientIDChanging(value);
					this.SendPropertyChanging();
					this._TreasureRecipientID = value;
					this.SendPropertyChanged("TreasureRecipientID");
					this.OnTreasureRecipientIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DealBeginTime", DbType="DateTime NOT NULL")]
		public System.DateTime DealBeginTime
		{
			get
			{
				return this._DealBeginTime;
			}
			set
			{
				if ((this._DealBeginTime != value))
				{
					this.OnDealBeginTimeChanging(value);
					this.SendPropertyChanging();
					this._DealBeginTime = value;
					this.SendPropertyChanged("DealBeginTime");
					this.OnDealBeginTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DealEndTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> DealEndTime
		{
			get
			{
				return this._DealEndTime;
			}
			set
			{
				if ((this._DealEndTime != value))
				{
					this.OnDealEndTimeChanging(value);
					this.SendPropertyChanging();
					this._DealEndTime = value;
					this.SendPropertyChanged("DealEndTime");
					this.OnDealEndTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DealStatus", DbType="NChar(50)")]
		public string DealStatus
		{
			get
			{
				return this._DealStatus;
			}
			set
			{
				if ((this._DealStatus != value))
				{
					this.OnDealStatusChanging(value);
					this.SendPropertyChanging();
					this._DealStatus = value;
					this.SendPropertyChanged("DealStatus");
					this.OnDealStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RemarkSToR", DbType="NVarChar(MAX)")]
		public string RemarkSToR
		{
			get
			{
				return this._RemarkSToR;
			}
			set
			{
				if ((this._RemarkSToR != value))
				{
					this.OnRemarkSToRChanging(value);
					this.SendPropertyChanging();
					this._RemarkSToR = value;
					this.SendPropertyChanged("RemarkSToR");
					this.OnRemarkSToRChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RemarkRToS", DbType="NVarChar(MAX)")]
		public string RemarkRToS
		{
			get
			{
				return this._RemarkRToS;
			}
			set
			{
				if ((this._RemarkRToS != value))
				{
					this.OnRemarkRToSChanging(value);
					this.SendPropertyChanging();
					this._RemarkRToS = value;
					this.SendPropertyChanged("RemarkRToS");
					this.OnRemarkRToSChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScoreSToR", DbType="VarChar(50)")]
		public string ScoreSToR
		{
			get
			{
				return this._ScoreSToR;
			}
			set
			{
				if ((this._ScoreSToR != value))
				{
					this.OnScoreSToRChanging(value);
					this.SendPropertyChanging();
					this._ScoreSToR = value;
					this.SendPropertyChanged("ScoreSToR");
					this.OnScoreSToRChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScoreRToS", DbType="VarChar(50)")]
		public string ScoreRToS
		{
			get
			{
				return this._ScoreRToS;
			}
			set
			{
				if ((this._ScoreRToS != value))
				{
					this.OnScoreRToSChanging(value);
					this.SendPropertyChanging();
					this._ScoreRToS = value;
					this.SendPropertyChanged("ScoreRToS");
					this.OnScoreRToSChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeliveryAddressSponsorID", DbType="NVarChar(MAX)")]
		public string DeliveryAddressSponsorID
		{
			get
			{
				return this._DeliveryAddressSponsorID;
			}
			set
			{
				if ((this._DeliveryAddressSponsorID != value))
				{
					this.OnDeliveryAddressSponsorIDChanging(value);
					this.SendPropertyChanging();
					this._DeliveryAddressSponsorID = value;
					this.SendPropertyChanged("DeliveryAddressSponsorID");
					this.OnDeliveryAddressSponsorIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeliveryAddressRecipientID", DbType="NVarChar(MAX)")]
		public string DeliveryAddressRecipientID
		{
			get
			{
				return this._DeliveryAddressRecipientID;
			}
			set
			{
				if ((this._DeliveryAddressRecipientID != value))
				{
					this.OnDeliveryAddressRecipientIDChanging(value);
					this.SendPropertyChanging();
					this._DeliveryAddressRecipientID = value;
					this.SendPropertyChanged("DeliveryAddressRecipientID");
					this.OnDeliveryAddressRecipientIDChanged();
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
