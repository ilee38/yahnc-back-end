using System;
using System.Collections.Generic;

namespace HackerNewsReader;

public partial class Favorite
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? StoryId { get; set; }

    public string? StoryUrl { get; set; }

    public string? StoryTitle { get; set; }

    public DateTime CreatedAt { get; set; }
}
