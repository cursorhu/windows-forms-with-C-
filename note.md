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


