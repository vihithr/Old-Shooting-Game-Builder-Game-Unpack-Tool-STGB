[中文](#简介) | [English](#overview)

## 简介

旧版 Shmups GameBuilder（STGBuilder）项目文件的解包与还原工具，用于从 `.gamedat` 归档中提取资源并恢复工程结构。

## 功能

- 选择并解包 `.gamedat` 资源包，输出到同级目录的 `UnpackData` 文件夹
- 读取打包索引与随包的 `.sbd` 字符串表，恢复原始文件名与路径（Shift_JIS/CP932 编码）
- 显示进度条与状态提示，界面保持响应

## 环境要求

- Windows 10/11
- .NET Framework 4.8（目标框架）
- Visual Studio 2019+（如需重新编译）

## 快速开始

1. 编译或直接运行 `STGBUNPACK_SHARP.exe`。
2. 点击“选择.gamedat文件”，指向需要解包的归档。
3. 点击“解包”，等待进度完成。资源将写入源文件同目录下的 `UnpackData\`。

> 文件名解析依赖 `.sbd` 数据（Shift_JIS/CP932）。若出现乱码，可能是归档缺少对应字符串表。

## 构建指南

1. 使用 Visual Studio 打开 `STGBUNPACK_SHARP.sln`。
2. 确认目标框架为 .NET Framework 4.8，选择 Debug 或 Release 配置。
3. 生成解决方案，可执行文件位于 `bin\Debug\` 或 `bin\Release\`。

## 工作原理

- 跳过头部后读取文件数量，遍历索引获取长度、偏移与名称占位。
- 首个索引指向 `.sbd` 字符串块，利用正则匹配恢复完整文件路径与扩展名。
- 将每个文件写入 `UnpackData\`，自动创建所需目录。

## 目录结构

- `Form1.cs`：解包逻辑与事件处理
- `Form1.Designer.cs` / `Form1.resx`：窗体布局与资源
- `Program.cs`：程序入口
- `STGBUNPACK_SHARP.csproj`：项目配置（.NET Framework 4.8）

## 已知限制

- 仅验证旧版 Shmups GameBuilder 打包格式，其他变体或自定义包不保证兼容。
- 文件名依赖 `.sbd` 字符串表，缺失时会回退为占位名。

## 许可协议

本项目采用 MIT License，详见 `LICENSE`。

---

## Overview

This Windows Forms utility extracts and reconstructs legacy Shmups GameBuilder project assets from `.gamedat` archives.

## Features

- Unpack `.gamedat` archives into an `UnpackData` folder next to the source file
- Reads the embedded index plus companion `.sbd` string table to restore original file names and paths (Shift_JIS/CP932)
- Progress bar and status label keep the UI responsive during extraction

## Requirements

- Windows 10/11
- .NET Framework 4.8 (target framework)
- Visual Studio 2019+ (if rebuilding)

## Quick Start

1. Build or run `STGBUNPACK_SHARP.exe`.
2. Click “选择.gamedat文件” to select the target `.gamedat` archive.
3. Click “解包” and wait for completion. Files appear under `UnpackData\` beside the archive.

> File name recovery relies on the `.sbd` table (Shift_JIS/CP932). Garbled names likely mean the archive lacks proper string data.

## Build From Source

1. Open `STGBUNPACK_SHARP.sln` in Visual Studio.
2. Keep the target framework at .NET Framework 4.8 and pick Debug or Release.
3. Build the solution; executables land in `bin\Debug\` or `bin\Release\`.

## How It Works

- Reads the file count after the header, then iterates index entries for size, offset, and placeholder names.
- Uses the first index entry’s `.sbd` string block and regex matching to restore full paths and extensions.
- Writes each file to `UnpackData\`, creating directories as needed.

## Project Layout

- `Form1.cs`: unpack logic and UI events
- `Form1.Designer.cs` / `Form1.resx`: form layout and resources
- `Program.cs`: application entry point
- `STGBUNPACK_SHARP.csproj`: project configuration (.NET Framework 4.8)

## Known Limitations

- Validated only against legacy Shmups GameBuilder archives; other variants may not work.
- File names fall back to placeholders if the `.sbd` table is missing.

## License

MIT License; see `LICENSE`.

