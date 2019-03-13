>清空选项

    属性->Properties->Buttons中新增一个按钮
    把按钮的Kind属性设置为Close，在lookUpEdit的右边就有一个小的Close按钮
    然后监听ButtonClick方法，根据Button.Index判断是否是Close按钮的点击事件，如果是就把lookUpEdit的EditValue设置为null