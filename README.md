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

Format: One Zone ID of [these](https://nodatime.org/TimeZones)
Literal example: `"Europe/Rome"`

References:
 - [NodaTime.DateTimeZone](https://nodatime.org/2.4.x/api/NodaTime.DateTimeZone.html)
 - [IANA (TZDB) time zone information](https://nodatime.org/TimeZones)

## Duration

Format: `-?(D:hh|H):mm:ss(.fff)?`
Literal example: `"-123:07:53:10.019`

References:
 - [NodaTime.DateTimeZone](https://nodatime.org/2.4.x/api/NodaTime.Duration.html)
 - [Patterns for Duration values](https://nodatime.org/2.4.x/userguide/duration-patterns)

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

- [ ] DateTimeZone
- [x] Duration
- [ ] Instant
- [x] IsoDayOfWeek
- [ ] LocalDate
- [ ] LocalDateTime
- [ ] LocalTime
- [ ] Offset
- [ ] OffsetDate
- [ ] OffsetDateTime
- [ ] OffsetTime
- [ ] Period
- [ ] ZonedDateTime

## Documentation

- [ ] DateTimeZone
- [ ] Duration
- [x] Instant
- [x] IsoDayOfWeek
- [ ] LocalDate
- [ ] LocalDateTime
- [ ] LocalTime
- [ ] Offset
- [ ] OffsetDate
- [ ] OffsetDateTime
- [ ] OffsetTime
- [ ] Period
- [ ] ZonedDateTime