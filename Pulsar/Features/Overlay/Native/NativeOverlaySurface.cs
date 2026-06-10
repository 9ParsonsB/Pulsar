namespace Pulsar.Features.Overlay.Native;

using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

public sealed class NativeOverlaySurface(
    IOverlayStateService overlayStateService,
    OverlayConfiguration configuration) : Control
{
    private static readonly Typeface TitleTypeface = new("Noto Sans", FontStyle.Normal, FontWeight.Bold);
    private static readonly Typeface StrongTypeface = new("Noto Sans", FontStyle.Normal, FontWeight.SemiBold);
    private static readonly Typeface BodyTypeface = new("Noto Sans");

    private readonly IBrush panelBrush = Brush("#A0070D12");
    private readonly IBrush panelWarningBrush = Brush("#A0160B08");
    private readonly IBrush accentBrush = Brush("#FF73DCFF");
    private readonly IBrush warningBrush = Brush("#FFFF7658");
    private readonly IBrush textBrush = Brush("#FFF0FBFF");
    private readonly IBrush mutedBrush = Brush("#FF9EC6D2");
    private readonly IBrush alertBrush = Brush("#FFFF9F88");
    private readonly IPen panelPen = new Pen(Brush("#4873DCFF"), 1);
    private readonly IPen warningPen = new Pen(Brush("#88FF7658"), 1);
    private readonly IPen dividerPen = new Pen(Brush("#24FFFFFF"), 1);

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var snapshot = overlayStateService.GetSnapshot();
        var layout = new OverlayLayout(context, configuration, Bounds.Width, Bounds.Height);

        DrawCommander(layout, snapshot);
        DrawDocking(layout, snapshot);
        DrawFuel(layout, snapshot);
        DrawBiology(layout, snapshot);
        DrawExploration(layout, snapshot);
        DrawAlerts(layout, snapshot);
    }

    private void DrawCommander(OverlayLayout layout, OverlaySnapshot snapshot)
    {
        var shipLabel = JoinNonEmpty(" | ", snapshot.Ship, snapshot.ShipName, snapshot.ShipIdent);
        var routeLabel = string.IsNullOrWhiteSpace(snapshot.Body)
            ? snapshot.System ?? "---"
            : $"{snapshot.System ?? "---"} - {snapshot.Body}";

        layout.Panel(92, panelBrush, panelPen, ctx =>
        {
            DrawText(ctx, "Commander", 0, 0, 12, accentBrush, StrongTypeface, letterSpacing: true);
            DrawText(ctx, snapshot.Commander ?? "Waiting for data", 0, 21, 24, textBrush, TitleTypeface);
            DrawText(ctx, string.IsNullOrWhiteSpace(shipLabel) ? "No active ship" : shipLabel, 0, 53, 14, mutedBrush);
            DrawText(ctx, routeLabel, 0, 73, 14, mutedBrush);
        });
    }

    private void DrawDocking(OverlayLayout layout, OverlaySnapshot snapshot)
    {
        if (string.IsNullOrWhiteSpace(snapshot.DockingState) && string.IsNullOrWhiteSpace(snapshot.Station))
            return;

        var station = snapshot.DockingStation ?? snapshot.Station ?? "Unknown station";
        var stationType = string.IsNullOrWhiteSpace(snapshot.StationType) ? null : $" - {snapshot.StationType}";

        layout.Panel(70, panelBrush, panelPen, ctx =>
        {
            DrawText(ctx, "Docking", 0, 0, 12, accentBrush, StrongTypeface, letterSpacing: true);
            DrawText(ctx, snapshot.DockingState ?? "At Station", 0, 22, 17, textBrush, StrongTypeface);
            if (snapshot.LandingPad is not null)
                DrawText(ctx, $"Pad {snapshot.LandingPad}", 300, 22, 17, textBrush, StrongTypeface, TextAlignment.Right, 84);
            DrawText(ctx, station + stationType, 0, 47, 14, mutedBrush);
        });
    }

    private void DrawFuel(OverlayLayout layout, OverlaySnapshot snapshot)
    {
        layout.Panel(86, snapshot.LowFuel ? panelWarningBrush : panelBrush, snapshot.LowFuel ? warningPen : panelPen, ctx =>
        {
            var fuelMain = snapshot.FuelMain?.ToString("0.0", CultureInfo.InvariantCulture) ?? "--";
            var fuelCapacity = snapshot.FuelCapacity?.ToString("0.0", CultureInfo.InvariantCulture) ?? "--";
            var percent = snapshot.FuelPercent.ToString("0.0", CultureInfo.InvariantCulture);
            var destination = snapshot.Destination ?? "No destination";
            var scoop = snapshot.FuelScooping && snapshot.FuelSecondsRemaining is not null
                ? $"Scoop {snapshot.FuelSecondsRemaining}s"
                : null;

            DrawText(ctx, "Fuel", 0, 0, 12, snapshot.LowFuel ? warningBrush : accentBrush, StrongTypeface, letterSpacing: true);
            DrawText(ctx, $"{fuelMain} / {fuelCapacity}t", 0, 22, 17, textBrush, StrongTypeface);
            DrawText(ctx, $"{percent}%", 300, 22, 17, textBrush, StrongTypeface, TextAlignment.Right, 84);

            var meter = new Rect(0, 49, 384, 10);
            ctx.DrawRectangle(Brush("#20FFFFFF"), null, meter, 5, 5);
            var width = Math.Clamp((double)snapshot.FuelPercent, 0, 100) / 100d * meter.Width;
            if (width > 0)
                ctx.DrawRectangle(snapshot.LowFuel ? warningBrush : accentBrush, null, meter.WithWidth(width), 5, 5);

            DrawText(ctx, destination, 0, 66, 13, mutedBrush);
            if (scoop is not null)
                DrawText(ctx, scoop, 260, 66, 13, mutedBrush, BodyTypeface, TextAlignment.Right, 124);
        });
    }

    private void DrawBiology(OverlayLayout layout, OverlaySnapshot snapshot)
    {
        if (string.IsNullOrWhiteSpace(snapshot.BiologySpecies))
            return;

        layout.Panel(78, panelBrush, panelPen, ctx =>
        {
            DrawText(ctx, "Biology", 0, 0, 12, accentBrush, StrongTypeface, letterSpacing: true);
            DrawText(ctx, snapshot.BiologySpecies, 0, 22, 17, textBrush, StrongTypeface);
            DrawText(ctx, snapshot.BiologyGenus ?? "Unknown genus", 0, 47, 14, mutedBrush);
            DrawText(ctx, $"Sample {snapshot.BiologySample ?? 0}/3", 260, 47, 14, mutedBrush, BodyTypeface, TextAlignment.Right, 124);
            DrawText(ctx, $"Next sample distance: {snapshot.BiologyRequiredDistance ?? 0}m", 0, 64, 12, mutedBrush);
        });
    }

    private void DrawExploration(OverlayLayout layout, OverlaySnapshot snapshot)
    {
        if (snapshot.HighValueBodies.Count == 0)
            return;

        var bodyCount = Math.Min(snapshot.HighValueBodies.Count, 5);
        layout.Panel(70 + bodyCount * 42, panelBrush, panelPen, ctx =>
        {
            DrawText(ctx, "Exploration", 0, 0, 12, accentBrush, StrongTypeface, letterSpacing: true);
            DrawText(ctx, snapshot.CurrentSystemScan ?? "No system", 0, 22, 17, textBrush, StrongTypeface);
            DrawText(ctx, $"{snapshot.BodiesScanned}/{snapshot.TotalBodies}", 300, 22, 17, textBrush, StrongTypeface, TextAlignment.Right, 84);
            DrawText(ctx, $"Estimated value: {snapshot.EstimatedSystemValue:N0} cr", 0, 47, 13, mutedBrush);

            var y = 70d;
            foreach (var body in snapshot.HighValueBodies.Take(5))
            {
                ctx.DrawLine(dividerPen, new Point(0, y - 8), new Point(384, y - 8));
                DrawText(ctx, body.Name ?? "Unknown body", 0, y, 14, textBrush, StrongTypeface);
                DrawText(ctx, $"{body.EstimatedValue:N0} cr", 230, y, 13, mutedBrush, BodyTypeface, TextAlignment.Right, 154);
                var detail = $"{body.Class ?? "Unknown"} - {body.DistanceFromArrivalLs:0} ls";
                if (body.Terraformable)
                    detail += " - terraformable";
                DrawText(ctx, detail, 0, y + 19, 12, mutedBrush);
                y += 42;
            }
        });
    }

    private void DrawAlerts(OverlayLayout layout, OverlaySnapshot snapshot)
    {
        if (snapshot.Alerts.Count == 0)
            return;

        layout.Panel(28 + snapshot.Alerts.Count * 21, panelWarningBrush, warningPen, ctx =>
        {
            DrawText(ctx, "Alerts", 0, 0, 12, warningBrush, StrongTypeface, letterSpacing: true);
            var y = 23d;
            foreach (var alert in snapshot.Alerts)
            {
                DrawText(ctx, alert, 0, y, 15, alertBrush, StrongTypeface);
                y += 21;
            }
        });
    }

    private static void DrawText(
        DrawingContext context,
        string? text,
        double x,
        double y,
        double size,
        IBrush brush,
        Typeface? typeface = null,
        TextAlignment alignment = TextAlignment.Left,
        double maxWidth = 384,
        bool letterSpacing = false)
    {
        var display = letterSpacing ? (text ?? string.Empty).ToUpperInvariant() : text ?? string.Empty;
        var formatted = new FormattedText(
            display,
            CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            typeface ?? BodyTypeface,
            size,
            brush)
        {
            MaxTextWidth = Math.Max(1, maxWidth),
            MaxTextHeight = size * 1.45,
            TextAlignment = alignment,
            Trimming = TextTrimming.CharacterEllipsis
        };

        context.DrawText(formatted, new Point(x, y));
    }

    private static string JoinNonEmpty(string separator, params string?[] values)
    {
        return string.Join(separator, values.Where(value => !string.IsNullOrWhiteSpace(value)));
    }

    private static IBrush Brush(string color)
    {
        return new SolidColorBrush(Color.Parse(color));
    }

    private sealed class OverlayLayout(
        DrawingContext rootContext,
        OverlayConfiguration configuration,
        double actualWidth,
        double actualHeight)
    {
        private readonly double scale = Math.Max(0.5, configuration.Scale);
        private double cursorY = configuration.Padding;

        public void Panel(double contentHeight, IBrush brush, IPen pen, Action<DrawingContext> draw)
        {
            var x = configuration.Padding;
            var width = Math.Min(configuration.PanelWidth, Math.Max(1, actualWidth - configuration.Padding * 2));
            var height = contentHeight + configuration.PanelPadding * 2;
            if (cursorY + height > actualHeight)
                return;

            var rect = new Rect(x, cursorY, width, height);
            rootContext.DrawRectangle(brush, pen, rect, 14, 14);

            using (rootContext.PushTransform(Matrix.CreateScale(scale, scale)))
            using (rootContext.PushTransform(Matrix.CreateTranslation(
                       (x + configuration.PanelPadding) / scale,
                       (cursorY + configuration.PanelPadding) / scale)))
            {
                draw(rootContext);
            }

            cursorY += height + configuration.PanelGap;
        }
    }
}
