# BottomSheetFreezeRepro

Minimal .NET MAUI repro for a bottom sheet freeze issue on iOS when the bottom sheet **grabber handle** reaches the transparent tab bar zone.

## Issue Description

On iOS 18.2+ with transparent tab bars (liquid glass), dragging a bottom sheet downward causes the app to freeze completely when the **grabber handle** (drag handle at top of sheet) reaches the transparent tab bar zone. 

**Critical Finding**: The freeze occurs specifically when the **grabber handle** overlaps with the tab bar gesture zone, NOT when the content reaches it. This indicates a **gesture recognizer conflict** between the bottom sheet's drag gesture and iOS's tab bar touch handling.

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

1. Launch the app on iOS 18.2+ device/simulator with transparent tab bar enabled
2. Open the bottom sheet (opens automatically)
3. **SLOWLY** drag the bottom sheet downward toward the bottom of the screen
4. **Observe**: App freezes when the **grabber handle** (at top of sheet) reaches the transparent tab bar zone
5. **Important**: The freeze happens when the GRABBER reaches the tab bar, not when the content does
6. No logs, no errors - complete UI freeze requiring force quit

## Workaround (PARTIAL)

Uncomment the workaround section in `ReproFeatureIdentifyContentView.xaml`:
- Wrap content in a container with `MaximumHeightRequest` (e.g., 300-600)
- This limits how far down the bottom sheet can be dragged before fully collapsing
- **Note**: This is a partial workaround - if the grabber still reaches the tab bar during drag, freeze can still occur
- The workaround works by ensuring the sheet collapses before the grabber reaches the critical zone

## Key Observations liquid glass)
- **Only when dragging slowly** near the tab bar zone
- **No errors or logs** - silent freeze at framework level
- **CRITICAL**: Freeze occurs when **grabber handle** reaches tab bar zone, NOT when content does
- **Content height irrelevant**: Even with content bounded by `MaximumHeightRequest`, if the grabber reaches the tab bar, it still freezes
- **Gesture conflict**: Appears to be conflict between bottom sheet drag gesture and iOS tab bar gesture recognizers
- **Content dependency**: Simpler content may not trigger the issue
- **Timing/position specific**: Freeze occurs when content measurement overlaps tab bar gesture area

## Root Cause Hypothesis
**Gesture recognizer conflict** (not layout measurement):

1. Bottom sheet has a drag gesture recognizer attached to the grabber handle
2. iOS tab bar has its own gesture recognizers for tab selection and interactions
3. When grabber handle overlaps with transparent tab bar zone during drag:
   - Both gesture recognizers become active simultaneously
   - Gesture conflict/deadlock occurs between bottom sheet drag and tab bar gestures
   - Framework-level freeze with no error logging

**Evidence**:
- Freeze position correlates with **grabber handle** reaching tab bar, not content
- Content height constraints (MaximumHeightRequest) don't prevent freeze if grabber still reaches tab bar
- Only affects transparent tab bars where gestures extend into "transparent" zone
- Silent freeze indicates low-level gesture handling deadlocknizer
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
<!-- Page configuration that exposes the issue -->the grabber handle passes through the transparent tab bar zone.

## Actual Behavior

App freezes completely (silent deadlock) when the bottom sheet's **grabber handle** reaches the transparent tab bar gesture zone during drag operation. No errors, no logs - requires force quit to recover.

## Potential Solutions for MAUI/Syncfusion Team

1. **Gesture priority handling**: Ensure bottom sheet drag gesture takes priority over tab bar gestures during active drag
2. **Gesture zone exclusion**: Exclude transparent tab bar zone from bottom sheet gesture recognition during collapse
3. **Safe area awareness**: Make bottom sheet gesture handling aware of iOS safe areas and adjust gesture boundaries
4. **Gesture state isolation**: Prevent gesture conflicts when multiple recognizers overlap in transparent UI zones
</ContentPage>
```

## Expected Behavior

Bottom sheet should drag smoothly to closed state regardless of content height, without freezing when content overlaps with tab bar zone.

## Actual Behavior

App freezes completely when dragging bottom sheet with unbounded content that extends into iOS tab bar safe area zone.
