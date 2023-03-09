# dotnet-tool∫-gist

dotnet-tool-gist is a command-line tool for using GitHub Gists as a package repository. With dotnet-tool-gist, you can easily install, restore and manage packages directly from the .NET command-line interface (CLI), using Gists as your package repository.

- [dotnet-tool∫-gist](#dotnet-tool-gist)
  - [Introduction](#introduction)
  - [Installation](#installation)
  - [Usage](#usage)
    - [Add](#add)
    - [Remove](#remove)
    - [List](#list)
    - [Update](#update)
    - [Restore](#restore)
  - [How it works](#how-it-works)
  - [License](#license)

## Introduction

There are several reasons why you might choose to use a package manager based on **GitHub Gist** over another one like *NuGet*:

- **Simplicity**: GitHub Gist is a simple way to share code snippets and small applications. It requires no setup, and you can create a gist with just a few clicks. This makes it a great option if you need to share code quickly and easily.

- **Version Control**: GitHub Gist provides version control, allowing you to track changes to your code over time. This makes it easy to collaborate with others and keep track of changes to your code.

- **Lightweight**: GitHub Gist is lightweight and doesn't have any dependencies. This makes it a great option if you need to quickly add a small piece of functionality to your project without adding unnecessary bloat.

- **Community**: GitHub Gist is part of the GitHub ecosystem, which has a large and active community of developers. This means that there are many resources available if you need help with your code, and you can easily find examples of how others have solved similar problems.

However, it's important to note that GitHub Gist is not a full-fledged package manager like *NuGet*. It's best suited for sharing small code snippets or simple applications, rather than managing complex dependencies for larger projects. Ultimately, the choice of which package manager to use will depend on your specific needs and the requirements of your project.

## Installation

To install dotnet-tool-gist, you'll need to have the .NET Core SDK installed on your machine. If you don't have it already, you can download it from the official .NET website: https://dotnet.microsoft.com/download.

Once you have the .NET Core SDK installed, you can install dotnet-tool-gist using the following command:

```bash
dotnet tool install -g dotnet-tool-gist
```

This will install the latest version of dotnet-tool-gist globally on your machine, so you can use it from anywhere in your terminal.

To verify that dotnet-tool-gist was insdotnettalled correctly, you can run the following command:

```bash
dotnet gist --version
```

If everything was installed correctly, you should see the version number of dotnet-tool-gist printed to the terminal.

## Usage

Once you have dotnet-tool-gist installed, you can use it to manage your GitHub Gists directly from the .NET CLI. Here are some examples of how you can use dotnet-tool-gist:

### Add

Add a gist reference to a project.

```bash
dotnet gist [<project>] add <gist-id> [--version <version>] [--file <file>] [--out <out>]
```

- `<project>`: The project file to add the reference. If not specified, the current directory must be a project.
- `<gist-id>`: The gist id.
- `<version>`: The gist version. If not specified, the latest version will be used.
- `<file>`: The file glob pattern to add. If not specified, all files will be added.
- `<out>`: The output directory. If not specified, the files will be added to the project directory: `gist/<gist-id>/<gist-version>`.

Some examples:

```
dotnet gist add <gist-id>
dotnet gist MyProject/MyProject.csproj add <gist-id>
dotnet gist MyProject/MyProject.csproj add <gist-id> --version <version>
dotnet gist MyProject/MyProject.csproj add <gist-id> --version <version> --file *.cs
dotnet gist add <gist-id> --version <version> --file *.cs --out ReferencedCode/
```

### Remove

Remove a gist reference from a project.

```bash
dotnet gist [<project>] remove <gist-id>
```

- `<project>`: The project file to remove the reference. If not specified, the current directory must be a project.
- `<gist-id>`: The gist id.

### List

List all gist references from a project.

```bash
dotnet gist [<project>] list
```

- `<project>`: The project file to list the references. If not specified, the current directory must be a project.

### Update

Update a gist reference version from a project.

```bash
dotnet gist [<project>] update <gist-id> [--version <version>]
```

- `<project>`: The project file to update the reference. If not specified, the current directory must be a project.
- `<gist-id>`: The gist id.
- `<version>`: The gist version. If not specified, the latest version will be used.

To update all references to the latest version:

```bash
dotnet gist update --all
```

### Restore

Restore all gist references from a project.

```bash
dotnet gist [<project>] restore
```

## How it works

Dotnet gist uses the [Github Gists API](https://developer.github.com/v3/gists/) to get the gist files and add them to your project.

Each gist reference is added to the project as a `GistReference` item under `ProjectExtensions` tag (a tag ignored by MsBuild and created to extend our dotnet projects) with the following metadata:

- `Id`: The gist id.
- `Version`: The gist version.
- `FilePattern`: The file glob pattern used to add the files.
- `OutputPath`: The output directory.

There is an internal cache of the gist files that is used to avoid downloading the same gist multiple times. The cache is stored in the `gist` folder under the `obj` project folder.

## License

dotnet-tool-gist is licensed under the [MIT License](LICENSE).
