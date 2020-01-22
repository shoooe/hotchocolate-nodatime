## Introduction

Adds support for [NodaTime](https://github.com/nodatime/nodatime) types in [Hot Chocolate](https://github.com/ChilliCream/hotchocolate).

## Usage

### .NET Core

Just call `AddNodaTime` on your schema builder like so:

```c#
SchemaBuilder.New()
    // ...
    .AddNodaTime()
    .Create();
```