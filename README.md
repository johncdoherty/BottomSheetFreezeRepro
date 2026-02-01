# BottomSheetFreezeRepro

Minimal .NET MAUI repro for bottom sheet freeze.

## Issue

Dragging a bottom sheet downward freezes the app when the grabber handle reaches the tab bar zone.

## Environment

- .NET MAUI 10.0 (net10.0-ios)
- iOS 18.2+
- Syncfusion.Maui.Toolkit.BottomSheet
- iOS only (Android not affected)



## Repro Steps

1. Launch on iOS 18.2+ device/simulator
2. Open bottom sheet (opens automatically)
3. Slowly drag sheet downward
4. **Observe**: Freeze when grabber handle reaches tab bar zone
5. No errors logged - requires force quit

## Key Finding

Freeze occurs when **grabber handle** reaches tab bar, not when content does.

## Workaround

See `ReproFeatureIdentifyContentView.xaml` for workaround code (commented out):

```xaml
<Border MaximumHeightRequest="300">
    <ScrollView>
        <!-- Content -->
    </ScrollView>
</Border>
```

Limits sheet collapse distance so grabber doesn't reach tab bar zone.

## Configuration

```xaml
<ContentPage SafeAreaEdges="None">
    <Grid SafeAreaEdges="None">
        <bottomSheet:SfBottomSheet ... />
    </Grid>
</ContentPage>
```
