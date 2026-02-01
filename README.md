# BottomSheetFreezeRepro

Minimal .NET MAUI repro for a bottom sheet freeze issue on iOS when content extends into the tab bar zone.

## Issue Description

On iOS 18.2+ with transparent tab bars, when a bottom sheet contains content that can extend into the tab bar zone (safe area), dragging the sheet slowly downward causes the app to freeze completely. This appears to be a layout measurement or gesture conflict between the bottom sheet's drag gesture and iOS's tab bar touch handling.

## Environment

- **.NET MAUI**: 10.0 (net10.0-ios)
- **iOS**: 18.2+ (with liquid glass transparent tab bar)
- **Bottom Sheet**: Syncfusion.Maui.Toolkit.BottomSheet
- **Platform**: iOS only (Android not affected)

## Setup

Restore and build:
```bash
dotnet restore
dotnet build
```

## Run (iOS)
```bash
dotnet build -t:Run -f net10.0-ios
```

## Repro Steps (BROKEN - Demonstrates Freeze)

1. Launch the app on iOS 18.2+ device/simulator
2. Open the bottom sheet (opens automatically)
3. **SLOWLY** drag the bottom sheet downward toward the tab bar
4. **Observe**: App freezes when content reaches the transparent tab bar zone
5. No logs, no errors - complete UI freeze requiring force quit

## Workaround (FIXED)

Uncomment the workaround section in `ReproFeatureIdentifyContentView.xaml` to see the fix:
- Wrap content in a container with `MaximumHeightRequest` (e.g., 600)
- This prevents content from extending unbounded into the tab bar zone
- Can use either `Border`, `Grid`, or `ScrollView` as the bounded container

## Key Observations

- **Only affects iOS** with transparent tab bars (iOS 18.2+)
- **Only when dragging slowly** near the tab bar zone
- **No errors or logs** - silent freeze at framework level
- **Content dependency**: Simpler content may not trigger the issue
- **Timing/position specific**: Freeze occurs when content measurement overlaps tab bar gesture area

## Root Cause Hypothesis

Layout measurement conflict when:
1. Bottom sheet content extends into iOS safe area (tab bar zone)
2. User drags sheet downward
3. iOS attempts to measure/layout content that overlaps with tab bar gesture recognizer
4. MAUI's layout engine enters infinite loop or deadlock with iOS gesture handling

## Workaround Code

### Option 1: Border with MaximumHeightRequest
```xaml
<Border MaximumHeightRequest="600">
    <!-- Your unbounded content -->
</Border>
```

### Option 2: ScrollView with MaximumHeightRequest
```xaml
<ScrollView MaximumHeightRequest="600">
    <!-- Your unbounded content -->
</ScrollView>
```

### Option 3: Grid with MaximumHeightRequest
```xaml
<Grid MaximumHeightRequest="600">
    <!-- Your unbounded content -->
</Grid>
```

## Related Configuration

```xaml
<!-- Page configuration that exposes the issue -->
<ContentPage ios:Page.UseSafeArea="False">
    <Grid IgnoreSafeArea="True">
        <!-- Bottom sheet here -->
    </Grid>
</ContentPage>
```

## Expected Behavior

Bottom sheet should drag smoothly to closed state regardless of content height, without freezing when content overlaps with tab bar zone.

## Actual Behavior

App freezes completely when dragging bottom sheet with unbounded content that extends into iOS tab bar safe area zone.
