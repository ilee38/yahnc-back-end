using System;
using System.Collections.Generic;

namespace HackerNewsReader;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? About { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Picture { get; set; }
}
