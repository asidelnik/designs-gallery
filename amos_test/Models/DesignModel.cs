namespace amos_test.Models;

public class DesignModel
{
  public int? Id { get; set; }
  public string? DataNew { get; set; }
  public string? PreviewImage { get; set; }
}

public class ImageProps
{
  public string? url { get; set; }
}

public class AdminPreviewOldFormat
{
  public string? src { get; set; }
  public List<string>? matrix { get; set; }
  public string? filterName { get; set; }
  public int? indexInPage { get; set; }
}

public class TransformData
{
  public int? x { get; set; }
  public int? y { get; set; }
  public int? scale { get; set; }
}

public class AdminPreviewPhotoSlotProps
{
  public int? rotate { get; set; }
  public bool? isFlipped { get; set; }
  public string? fileSource { get; set; }
  public string? filterType { get; set; }
  public TransformData? transformData { get; set; }
}

public class TextProps
{
  public TextContent? text { get; set; }
  public string? align { get; set; }
  public string? color { get; set; }
  public string? fontID { get; set; }
  public int? rotate { get; set; }
  public double? fontSize { get; set; }
  public int? lineHeight { get; set; }
  public double? letterSpacing { get; set; }
}

public class Element
{
  public double? x { get; set; }
  public double? y { get; set; }
  public string? id { get; set; }
  public string? type { get; set; }
  public int? width { get; set; }
  public int? height { get; set; }
  public ImageProps? imageProps { get; set; }
  public AdminPreviewOldFormat? adminPreviewOldFormat { get; set; }
  public AdminPreviewPhotoSlotProps? adminPreviewPhotoSlotProps { get; set; }
  public TextProps? textProps { get; set; }
}

public class PageText
{
  public TextContent? text { get; set; }
}

public class TextContent
{
  public string? type { get; set; }
  public List<TextContent>? content { get; set; }
  public string? text { get; set; }
}

public class Page
{
  public List<Element>? elements { get; set; }
  public Dictionary<string, PageText>? generatedElementsValues { get; set; }
}

public class DataNew
{
  public string? id { get; set; }
  public Page? backPage { get; set; }
  public Page? frontPage { get; set; }
  public Page? innerPage { get; set; }
  public string? previewColor { get; set; }
  public string? previewImageUrl { get; set; }
  public int? backgroundImageId { get; set; }
  public string[]? suggestedTextColors { get; set; }
}