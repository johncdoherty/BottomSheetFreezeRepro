# Syncfusion SfBottomSheet iOS Tab Bar Freeze Issue

## Issue Summary

Syncfusion's `SfBottomSheet` freezes when dragging to close on iOS when the grabber handle reaches the iOS tab bar zone. This appears to be a gesture conflict between the bottom sheet's drag gesture and iOS tab bar gestures.

## Environment

- **.NET MAUI:** 10.0.30
- **Syncfusion.Maui.Toolkit:** 1.0.8
- **iOS Versions Tested:** 18.x, 18.2.6
- **Devices:** iPhone with tab bar navigation

## Reproduction Steps

1. Open the app on iOS device with tab bar
2. Tap toggle button to select **Syncfusion** mode
3. Tap toggle to show **BROKEN** scenario
4. Tap "Open bottom sheet"
5. Slowly drag the grabber handle downward
6. **Observe:** App freezes when grabber reaches tab bar zone (no crash, no logs, complete deadlock)

## Key Findings

### Pure MAUI Implementation
✅ **Works correctly on all iOS versions**
- Uses native MAUI `PanGestureRecognizer`
- No freeze when dragging near tab bar
- Confirms this is NOT a MAUI platform bug

### Syncfusion Implementation
❌ **Freezes on all tested iOS versions (18.x, 18.2.6)**
- Freeze occurs when **grabber handle** reaches tab bar zone
- Content position is irrelevant - it's specifically the grabber
- Pure MAUI works fine → This is a Syncfusion bug

## Workaround (Verified Working)

Wrap content in a `ScrollView` with `MaximumHeightRequest` to constrain height:

```xaml
<bottomSheet:SfBottomSheet.BottomSheetContent>
    <ContentView SafeAreaEdges="Container, Container, Container, SoftInput">
        <Grid RowDefinitions="Auto,Auto,Auto,*">
            <!-- Header -->
            <Label Grid.Row="0" Text="Title" />
            
            <!-- Constrained scrollable content -->
            <ScrollView Grid.Row="1" 
                        MaximumHeightRequest="600"
                        HorizontalOptions="Fill">
                <Border>
                    <!-- Your expandable content here -->
                    <VerticalStackLayout>
                        <!-- Filters, buttons, etc -->
                    </VerticalStackLayout>
                </Border>
            </ScrollView>
            
            <!-- Footer/List -->
            <CollectionView Grid.Row="2" ItemsSource="{Binding Items}" />
        </Grid>
    </ContentView>
</bottomSheet:SfBottomSheet.BottomSheetContent>
```

### Key Points:
- ✅ `MaximumHeightRequest` must be on **ScrollView** (not on Border/Grid inside)
- ✅ Use `600` or appropriate height for your content
- ✅ Works on iOS 18.x and 18.2.6
- ✅ Prevents bottom sheet from extending too far, keeping grabber away from tab bar zone

## Demo App Features

### Toggle Between Implementations
- **🔷 Syncfusion / 📱 Pure MAUI** - Compare Syncfusion vs native MAUI implementation
- **🔴 BROKEN / 🟢 FIXED** - Toggle between broken scenario and working workaround (Syncfusion only)

### Test Scenarios
1. **Pure MAUI** - Proves MAUI gestures work correctly
2. **Syncfusion BROKEN** - Demonstrates the freeze issue
3. **Syncfusion FIXED** - Shows the workaround in action

## Configuration

The demo uses the same bottom sheet configuration as production apps:

```xaml
<bottomSheet:SfBottomSheet
    IsOpen="False"
    IsModal="True"
    AllowedState="FullExpanded"
    CollapseOnOverlayTap="True"
    CornerRadius="20"
    FullExpandedRatio="0.9"
    CollapsedHeight="0"
    State="FullExpanded">
```

## Root Cause Analysis

**Syncfusion Bug:** The `SfBottomSheet` grabber's gesture recognizer conflicts with iOS tab bar gesture recognizers when they overlap spatially. This causes a silent deadlock.

**Why Pure MAUI Works:** MAUI's `PanGestureRecognizer` properly handles gesture conflicts with system UI.

**Why Workaround Works:** Constraining content height prevents the bottom sheet from extending far enough for the grabber to reach the tab bar conflict zone.

## Reporting

This should be reported to **Syncfusion Support** with this demo project.

**Evidence:**
- ✅ Pure MAUI implementation works perfectly
- ❌ Syncfusion implementation freezes
- ✅ Workaround prevents freeze by limiting sheet expansion
- 🎯 Issue is in Syncfusion's gesture handling, not MAUI platform
