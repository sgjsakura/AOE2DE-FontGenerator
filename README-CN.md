# 帝国时代二决定版字体生成器

这个项目用于提供帝国时代二决定版游戏中使用的字体文件，可用于解决游戏中字符集不全的问题，或者优化文字显示效果。

**注意：遇到任何问题时，请先阅读“常见问题和解答”一节。**

## 如何使用这个工具

要使用这个工具替换游戏中的字体，请按照下列步骤操作：

1. 打开这个程序，设定一个或者多个字体及其关联到的字符集。
2. 单击“开始生成”按钮等待生成字体文件。
3. 复制生成的文件（主要是box和dds扩展名的文件，其他文件可忽略）到游戏安装目录的“resources/_common/fonts”文件夹替换原始文件。注意生成的文件可能比原来包含的文件多，这是正常现象。
4. 启动游戏，你将能在游戏大厅、科技树和游戏运行界面看到效果。注意进入游戏时的主界面并不会受影响。
5. 如果你要调整显示效果或者更改字体，请重复以上步骤。

## 常见问题和解答

#### 生成终止了，显示有错误

请仔细阅读错误消息，按照提示操作。如果你仍然有问题，请在这里为这个项目创建 Issue。

#### 程序崩溃了

请创建新的 Issue 并详细说明重现步骤，以便于作者修复问题。

#### 游戏里面的文字看起来太大/太小

这是因为你选择的字体不能正确按照请求的尺寸产生字形。请进入详细设置窗口，调整“字形尺寸缩放比例”一栏以便于调整生成的字形大小。特别请注意，调整主窗口的字形尺寸设置并不能解决这个问题。

#### 游戏里面的文字看起来垂直方向没对齐

这是因为你选择的字体提供的基线位置不正确。请进入详细设置窗口，调整“字形基线偏移比例”以调节文字在垂直方向的位置。

#### 有些字看起来像是乱码

这是因为生成的字形过大（虽然看起来不一定溢出界面）导致读取字形时显存数据受到影响。请参考“游戏里面的文字看起来太大/太小”一节中的办法解决这个问题。

#### 有些字变成了问号

这是因为字形丢失的缘故。你生成的字体文件中不包含这个要显示的字符，因此系统将它自动替换为了一个问号。请考虑换一个字体重新生成，或者生成时添加另一个字体补充这些丢失的字形。
