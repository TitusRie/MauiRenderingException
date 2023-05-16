using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Shapes;

namespace MauiRenderingException;

/// <summary>
/// Microsoft.UI.Xaml.UnhandledException
/// Exception Message: A cycle occurred while laying out the GUI.
/// Event Message: Layout cycle detected.  Layout could not complete.
/// </summary>
public class App : Application
{
    public App()
    {
        MainPage = new ContentPage
        {
            Content = new CollectionView
            {
                ItemsSource = new[] { "Test A", "Test B" },
                ItemTemplate = new DataTemplate(() =>
                {
                    var pathString = "M 96 14 82 0 48 34 14 0 0 14 34 48 0 82 14 96 48 62 82 96 96 82 62 48 Z";
                    var pathGeometry = new PathGeometry();
                    PathFigureCollectionConverter.ParseStringToPathFigureCollection(pathGeometry.Figures, pathString);
                    return new Frame
                    {
                        BorderColor = Colors.Red,
                        BackgroundColor = Colors.DarkRed,
                        HeightRequest = 80,
                        Content = new Grid
                        {
                            Children =
                            {
                                new Grid
                                {
                                    ColumnSpacing = 4,
                                    ColumnDefinitions = new ColumnDefinitionCollection(new ColumnDefinition(60)),
                                    Children =
                                    {
                                        new Frame
                                        {
                                            CornerRadius = 6,
                                            BorderColor = Colors.Transparent,
                                            Content =
                                                new Grid
                                                {
                                                    Children =
                                                    {
                                                        new Label
                                                        {
                                                            Text = "M",
                                                            // setting font size too big will cause Xaml.UnhandledException
                                                            FontSize = 100
                                                        }
                                                    }
                                                }
                                        },
                                        new Grid
                                        {
                                            Children =
                                            {
                                                new Frame
                                                {
                                                    CornerRadius = 6,
                                                    BorderColor = Colors.Transparent,
                                                    Content =
                                                        new Grid
                                                        {
                                                            Children =
                                                            {
                                                                new Grid
                                                                {
                                                                    Children =
                                                                    {
                                                                        new Microsoft.Maui.Controls.Shapes.Path(pathGeometry)
                                                                        {
                                                                            // not setting size small enough will cause Xaml.UnhandledException
                                                                            //WidthRequest =10,
                                                                            //HeightRequest = 10,
                                                                        },
                                                                    }
                                                                }
                                                            }
                                                        }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    };
                }),
            }
        };
    }
}

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
