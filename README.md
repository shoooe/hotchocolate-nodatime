# Introduction

Adds support for [NodaTime](https://github.com/nodatime/nodatime) types in [Hot Chocolate](https://github.com/ChilliCream/hotchocolate).

# Usage

## .NET Core

Just call `AddNodaTime` on your schema builder like so:

```c#
SchemaBuilder.New()
    // ...
    .AddNodaTime()
    .Create();
```

# Documentation

## DateTimeZone

Format: One Zone ID from [these](https://nodatime.org/TimeZones)

Literal example: `"Europe/Rome"`

References:
 - [NodaTime.DateTimeZone](https://nodatime.org/2.4.x/api/NodaTime.DateTimeZone.html)
 - [IANA (TZDB) time zone information](https://nodatime.org/TimeZones)

## Duration

Literal example: `"-123:07:53:10.019"`

References:
 - [NodaTime.Duration](https://nodatime.org/2.4.x/api/NodaTime.Duration.html)
 - [Patterns for Duration values](https://nodatime.org/2.4.x/userguide/duration-patterns)

## Instant

Literal example: `"2020-02-20T17:42:59Z"`

References:
 - [NodaTime.Instant](https://nodatime.org/2.4.x/api/NodaTime.Instant.html)
 - [Patterns for Instant values](https://nodatime.org/2.4.x/userguide/instant-patterns)

## IsoDayOfWeek

Literal example: `7`

References:
 - [NodaTime.IsoDayOfWeek](https://nodatime.org/2.4.x/api/NodaTime.IsoDayOfWeek.html)

## LocalDate

Literal example: `"2020-12-25"`

References:
 - [NodaTime.LocalDate](https://nodatime.org/2.4.x/api/NodaTime.LocalDate.html)
 - [Patterns for LocalDate values](https://nodatime.org/2.4.x/userguide/localdate-patterns)

## LocalDateTime

Literal example: `"2020-12-25T13:46:78"`

References:
 - [NodaTime.LocalDateTime](https://nodatime.org/2.4.x/api/NodaTime.LocalDateTime.html)
 - [Patterns for LocalDateTime values](https://nodatime.org/2.4.x/userguide/localdatetime-patterns)

## LocalDateTime

Literal examples: 
 - `"12:42:13"`
 - `"12:42:13.03101"`

References:
 - [NodaTime.LocalTime](https://nodatime.org/2.4.x/api/NodaTime.LocalTime.html)
 - [Patterns for LocalTime values](https://nodatime.org/2.4.x/userguide/localtime-patterns)

## OffsetDateTime

Literal examples: 
 - `"2020-12-25T13:46:78+02"`
 - `"2020-12-25T13:46:78+02:35"`

References:
 - [NodaTime.OffsetDateTime](https://nodatime.org/2.4.x/api/NodaTime.OffsetDateTime.html)
 - [Patterns for OffsetDateTime values](https://nodatime.org/2.4.x/userguide/offsetdatetime-patterns)

# Roadmap

## Implementation

- [x] DateTimeZone
- [x] Duration
- [x] Instant
- [x] IsoDayOfWeek
- [x] LocalDate
- [x] LocalDateTime
- [x] LocalTime
- [x] Offset
- [x] OffsetDate
- [x] OffsetDateTime
- [x] OffsetTime
- [x] Period
- [x] ZonedDateTime

## Tests

- [x] DateTimeZone
- [x] Duration
- [x] Instant
- [x] IsoDayOfWeek
- [x] LocalDate
- [x] LocalDateTime
- [x] LocalTime
- [ ] Offset
- [ ] OffsetDate
- [x] OffsetDateTime
- [ ] OffsetTime
- [ ] Period
- [ ] ZonedDateTime

## Documentation

- [x] DateTimeZone
- [x] Duration
- [x] Instant
- [x] IsoDayOfWeek
- [x] LocalDate
- [x] LocalDateTime
- [x] LocalTime
- [ ] Offset
- [ ] OffsetDate
- [x] OffsetDateTime
- [ ] OffsetTime
- [ ] Period
- [ ] ZonedDateTime