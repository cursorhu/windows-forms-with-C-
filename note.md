## chapter1
1. 项目结构
VS创建windows forms窗口程序，项目文件默认包含：
- Form1.cs -- 主窗口类的一部分定义(partitial)，实现控件的方法；默认使用CSharp窗体编辑器打开，可右键选择C#源码模式打开；可以改文件名和在窗口界面右键属性改窗口名，例如项目改为MainForm.
- Form1.Designer.cs -- 主窗口类的一部分定义(partitial)，实现控件的属性定义；窗口编辑器对控件的编辑操作直接反映到这个文件中，一般无需手写此文件的代码(EventHandler方法名如果注册后有改名需要在此手动修改). 此项目改名为MainForm.Designer.cs
- Program.cs -- 窗口程序的主函数，在这里Run主窗口类以启动对事件的监听。

2. 如何使用窗体编辑器设计控件布局，设置控件属性，定义控件的方法
- VS项目视图双击Form1.cs默认显示主窗口程序的GUI界面
- 右侧工具栏添加控件，例如Button, PictureBox
- 右键控件的属性，定义控件的Name(程序中的变量名)/Text(GUI显示的名称)等属性，对于可缩放控件例如PictureBox可定义Anchor(=跟随四面边框)，行为(=Zoom,内容缩放显示)，BorderStyle(=Fixed3D,控件边界三维效果)，对于其他特定控件有特定的属性设置。
- 双击控件，定义控件的方法(注册EventHandler回调)，对于Button/Checkbox这类接受用户输入的控件需要定义。

3. 使用Dialog打开文件并显示的知识点：
    DialogResult.OK表示用户点击了OK按钮，此时.jpg文件已选择，只需要dialog.OpenFile去读文件就能读到。
    (安全起见这里应该用try-catch异常处理，用户可能不选择图片文件导致程序崩溃)。

    代码中把图片读到bitmap对象中赋值给PictureBox.Image就完了，这个Image属性能同步更新到GUI界面吗？？
    熟悉MFC的可能有疑问了，MFC里面类成员的更新和GUI的同步要通过UpdateData(True)或UpdateData(False)去通信，Windows Forms如何实现这一同步通信？

    这属于windows forms的data binding(数据绑定)的概念，简单讲就是将属性和界面绑定，属性的值发生变化自动更新界面，反之界面的值变化自动更新属性。

    VS中右键可查看PictureBox控件类的定义：

    ```
    [DefaultBindingProperty("Image")]
    [DefaultProperty("Image")]
    public class PictureBox : Control, ISupportInitialize
    {
        [Bindable(true)]
        public Image Image { get; set; }
    }
    ```

    DefaultBindingProperty指定了绑定对象是Image
    Image的set和get方法: 隐藏Image数据，只提供方法。当引用Image对象（读Image）则调用get方法，如果是赋值Image则调用set方法
    因此对PictureBox.Image赋值时会调用set方法去更新Image属性，然后Binding会同步更新GUI界面的图片。

## chapter2
1. 项目属性

properties的AssemblyInfo.cs包含项目信息配置（即右键exe的详细信息内容）
GUI exe工具可以读取此信息，例如读AssemblyVersion并显示到窗口名：

```
System.Version ver = new System.Version(System.Windows.Forms.Application.ProductVersion);
this.Text = System.String.Format("ImageLoader {0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
```

关于String.Format的语法参考：
https://learn.microsoft.com/en-us/dotnet/api/system.string.format?view=net-8.0

需要特别注意：
控件的属性位于InitializeComponent内根据窗体编辑器自动生成的，不要在InitializeComponent内改动代码，否则一旦编辑了窗体编辑器就会被覆盖
在MainForm主类调用完InitializeComponent之后改代码，覆盖自动生成的属性代码。

2. try-catch异常处理
对于资源的申请一般要try-catch, 其作用：保证能处理各种异常，并确保异常发生时程序能退出，而不是崩溃或挂起影响用户操作。这也是try-catch优于使用ErrorCode处理异常的原因：
```
Exceptions provide a solution to these problems by forcing a programmer to deal
with them, and provide guarantees that the program will exit if they do not.
```
实测此项目中Bitmap OpenFile如果打开的文件不是图片类型，例如pdf文件，程序无响应导致系统死机后再退出；如果有try-catch就不会死机，弹窗报错误信息之后还可以继续选择图片并正常打开.

.Net默认使用ex作为Exception变量名，因为e已经默认为event事件名：
```
Event handlers in Visual Studio .NET tend to use
an “e” parameter for the event parameter to the call. To
ensure we avoid a conflict, we will use ex as a standard
variable name for an Exception object
```

## chapter3
Menu有两类：MainMenu和ContextMenu; VS2019中控件名分别是MenuStrip和ContextMenuStrip. 不叫MainMenu.
This class is not available in .NET Core 3.1 and later versions. Use ToolStripMenuItem instead, which replaces the MenuItem control.

添加快捷键实现快速选择和快速执行MenuItem：
- 使用“&+字符”定义MenuItem名称，可以支持快捷键“Alt+字符”选择此MenuItem(但不执行). 例如&File, E&xit
- 使用Shortcut property设置MenuItem, 支持快捷键例如Ctrl+Shortcut执行此MenuItem.

添加event handler有两种方式：
- 对于点击事件(Click)，直接双击MenuItem，自动生成对于的Click事件回调函数XXX_Click. 这种方式只支持鼠标或键盘快捷键.
- 更通用的事件添加方式：控件->右键->属性窗口->改为事件窗口->选择某事件并注册对应的回调函数. 

event hander回调函数的共享：
- 原理是使用index管理一类相关的MenuItem
- 多个MenuItem共享一个event hander，需要手动指定同一个回调函数(见代码_ChildClick).
- 注意_ChildClick和书中基于MenuItem实现不一样，ToolStripMenuItem没有Index属性，用Owner.Items.IndexOf()获取index.

Popup事件(打开时更新状态):
- ToolStripMenuItems没有Popup事件，使用DropDownOpening事件，用于子目录打开时做的操作(=Popup)
- ToolStripMenuItem和书中的MenuItem有实现差异：使用DropDownItems获取ToolStripMenuItem的子目录项；使用IndexOf获取index.

拷贝ToolStripMenuItems到ContextMenuItems(包括事件)：
- 书中的CloneMenu方法不适用于ToolStripMenuItems
- 我的尝试见DefineContextMenu(), 使用AddRange能创建ContextMenuItems，但会使ToolStripMenuItems消失
- 最终解决办法：在Design GUI工具栏创建ContextMenuItems后，直接ctrl+c复制ToolStripMenuItems，再ctrl+v粘贴到到ContextMenuItems.
参考https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/how-to-copy-toolstripmenuitems?view=netframeworkdesktop-4.8
注意ToolStripMenuItems的事件不会被复制粘贴到ContextMenuItems，需要在Design GUI手动注册事件.
例如ContextMenuItems要支持_ChildClick和_Popup事件：右键ContextMenuItems的ImageSizeMode目录项，切换到事件界面，添加_ChildClick和_Popup回调函数.

## chapter4
.Net高版本用StatusStrip取代了StatusBar, 参考：
https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/statusbar-control-overview-windows-forms?view=netframeworkdesktop-4.8

如何添加Panel:
https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.statusstrip?view=windowsdesktop-7.0
The default StatusStrip has no panels. To add panels to a StatusStrip, use the ToolStripItemCollection.AddRange method, or use the StatusStrip Items Collection Editor at design time to add, remove, or reorder items and modify properties.
直接在StatusBar添加多个子控件, 或者右键StatusBar选择“编辑项”，打开Collection Editor添加子控件项.
添加完毕可以在StatusBar属性的Items中查看Panel集合；多个Panel的间隔通过属性->Margin去调整

图片显示比例Image percent：
StatusStrip没有DrawItem，没实现这部分；StatusBarDrawItemEventArgs也用_ChildClick去替代.
状态栏中图片显示比例(Image percent)的计算逻辑修改为：显示图片大小和实际图片大小的比例，取决于当前pixBox的SizeMode.
原书的计算逻辑是窗口大小和实际图片的比例，如果显示比例是1:1但窗口大小比图片小，比例小于100%不符合逻辑, 应该显示为100%.
