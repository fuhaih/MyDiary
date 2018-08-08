# 常用需求
> 只显示年或者月面板
```csharp
dateEditTime.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
dateEditTime.Properties.ShowToday = false;
dateEditTime.Properties.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.YearView;
dateEditTime.Properties.VistaCalendarViewStyle = VistaCalendarViewStyle.YearView;
dateEditTime.Properties.Mask.EditMask = "yyyy-MM";
dateEditTime.Properties.Mask.UseMaskAsDisplayFormat = true;
```
以上代码是设置dateEdit只显示月面板，如果需要只显示年面板，把YearView替换为YearGroupView