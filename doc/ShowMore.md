# ShowMore

## Summary

The `ShowMore` control holds values into two state. It can preview [show a part of the content] and present the content.

## Platform support

| Feature                               | UWA | Android | iOS |
| --------------------------------------|:---:|:-------:|:---:|
| Expand animation                      |  X  |    X    |  X  |
| Retract animation                     |  X  |    X    |  X  |

## Prerequisites

First, you must add this into your ShowMore style.

```xml

    <!-- Color used in this exemple -->
	<Color x:Key="TransparentColor">Transparent</Color>
	<Color x:Key="Color2">#FFFFFFFF</Color>
	<Color x:Key="Color3">#FFE7A614</Color>
	<Color x:Key="Color4">#FF000000</Color>

	<Style TargetType="u:ShowMore"
		   x:Key="DefautlShowMoreStyle">
		<Setter Property="HorizontalAlignment"
				Value="Stretch" />
		<Setter Property="HorizontalContentAlignment"
				Value="Stretch" />
		<Setter Property="VerticalContentAlignment"
				Value="Stretch" />
		<Setter Property="Template">
			<Setter.Value>
				<ControlTemplate TargetType="u:ShowMore">
					<Border>
						<VisualStateManager.VisualStateGroups>
							<VisualStateGroup x:Name="OpenStates">
								<VisualState x:Name="Closed">
									<Storyboard>
										<DoubleAnimation x:Name="CloseAnimation"
														 Storyboard.TargetName="ShowMorePresenter"
														 Storyboard.TargetProperty="Height"
														 Duration="00:00:00.3"
														 To="{Binding PreviewHeight, RelativeSource={RelativeSource TemplatedParent}, Mode=OneWay}"
														 EnableDependentAnimation="True">
											<DoubleAnimation.EasingFunction>
												<CubicEase EasingMode="EaseOut" />
											</DoubleAnimation.EasingFunction>
										</DoubleAnimation>
										<ObjectAnimationUsingKeyFrames Storyboard.TargetName="GradientStopEffect"
																	   Storyboard.TargetProperty="Color">
											<DiscreteObjectKeyFrame KeyTime="0"
																	Value="{StaticResource Color2}" />
										</ObjectAnimationUsingKeyFrames>
									</Storyboard>
								</VisualState>
								<VisualState x:Name="Opened">
									<Storyboard>
										<DoubleAnimation x:Name="OpeningAnimation"
														 Storyboard.TargetName="ShowMorePresenter"
														 Storyboard.TargetProperty="Height"
														 Duration="00:00:00.3"
														 To="{Binding PresenterHeight, RelativeSource={RelativeSource TemplatedParent}, Mode=OneWay}"
														 EnableDependentAnimation="True">
											<DoubleAnimation.EasingFunction>
												<CubicEase EasingMode="EaseOut" />
											</DoubleAnimation.EasingFunction>
										</DoubleAnimation>
										<ObjectAnimationUsingKeyFrames Storyboard.TargetName="GradientStopEffect"
																	   Storyboard.TargetProperty="Color">
											<DiscreteObjectKeyFrame KeyTime="0"
																	Value="{StaticResource TransparentColor}" />
										</ObjectAnimationUsingKeyFrames>
									</Storyboard>
								</VisualState>

								<VisualStateGroup.Transitions>
									<VisualTransition From="Opened"
													  To="Closed">
										<Storyboard>
											<DoubleAnimation Storyboard.TargetName="ShowMorePresenter"
															 Storyboard.TargetProperty="Height"
															 Duration="00:00:00.3"
															 To="{Binding PreviewHeight, RelativeSource={RelativeSource TemplatedParent}, Mode=OneWay}"
															 EnableDependentAnimation="True">
												<DoubleAnimation.EasingFunction>
													<CubicEase EasingMode="EaseOut" />
												</DoubleAnimation.EasingFunction>
											</DoubleAnimation>
											<ObjectAnimationUsingKeyFrames Storyboard.TargetName="GradientStopEffect"
																		   Storyboard.TargetProperty="Color">
												<DiscreteObjectKeyFrame KeyTime="0"
																		Value="{StaticResource Color2}" />
											</ObjectAnimationUsingKeyFrames>
										</Storyboard>
									</VisualTransition>
									<VisualTransition From="Closed"
													  To="Opened">
										<Storyboard>
											<DoubleAnimation Storyboard.TargetName="ShowMorePresenter"
															 Storyboard.TargetProperty="Height"
															 Duration="00:00:00.3"
															 To="{Binding PresenterHeight, RelativeSource={RelativeSource TemplatedParent}, Mode=OneWay}"
															 EnableDependentAnimation="True">
												<DoubleAnimation.EasingFunction>
													<CubicEase EasingMode="EaseOut" />
												</DoubleAnimation.EasingFunction>
											</DoubleAnimation>
											<ObjectAnimationUsingKeyFrames Storyboard.TargetName="GradientStopEffect"
																		   Storyboard.TargetProperty="Color">
												<DiscreteObjectKeyFrame KeyTime="0"
																		Value="{StaticResource TransparentColor}" />
											</ObjectAnimationUsingKeyFrames>
										</Storyboard>
									</VisualTransition>
								</VisualStateGroup.Transitions>
							</VisualStateGroup>
						</VisualStateManager.VisualStateGroups>
						<Grid>
							<Grid.RowDefinitions>
								<RowDefinition Height="Auto" />
								<RowDefinition Height="Auto" />
							</Grid.RowDefinitions>

							<ContentPresenter x:Name="ShowMorePresenter"
											  Content="{TemplateBinding Content}"
											  ContentTemplate="{TemplateBinding ContentTemplate}"
											  ContentTemplateSelector="{TemplateBinding ContentTemplateSelector}"
											  Height="{Binding PreviewHeight, RelativeSource={RelativeSource TemplatedParent}, Mode=OneWay}"
											  ios:ClipsToBounds="True" />

							<Rectangle>
								<Rectangle.Fill>
									<LinearGradientBrush StartPoint="0,0"
														 EndPoint="0,1">
										<GradientStop Color="Transparent"
													  Offset="0" />
										<GradientStop x:Name="GradientStopEffect"
													  Color="{StaticResource Color2}"
													  Offset="1" />
									</LinearGradientBrush>
								</Rectangle.Fill>
							</Rectangle>

							<ToggleButton Grid.Row="1"
										  IsChecked="{Binding IsOpened, RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}"
										  HorizontalAlignment="Stretch"
										  HorizontalContentAlignment="Stretch"
										  Height="25"
										  Style="{StaticResource ShowMoreToggleButtonStyle}" />
						</Grid>
					</Border>
				</ControlTemplate>
			</Setter.Value>
		</Setter>
	</Style>

    <!-- This can be moved on the ToggleButton.xaml-->
	<Style TargetType="ToggleButton"
		   x:Key="ShowMoreToggleButtonStyle">
		<Setter Property="Template">
			<Setter.Value>
				<ControlTemplate TargetType="ToggleButton">
					<Grid x:Name="RootGrid"
						  Background="{TemplateBinding Background}">
						<VisualStateManager.VisualStateGroups>
							<VisualStateGroup x:Name="CommonStates">
								<VisualState x:Name="Normal">
									<Storyboard>
										<DoubleAnimation Storyboard.TargetName="DownArrowTransformation"
														 Storyboard.TargetProperty="Angle"
														 To="0"
														 Duration="0:0:0.3" />
									</Storyboard>
								</VisualState>
								<VisualState x:Name="PointerOver">
									<Storyboard>
										<ObjectAnimationUsingKeyFrames Storyboard.TargetProperty="Foreground"
																	   Storyboard.TargetName="ContentPresenter">
											<DiscreteObjectKeyFrame KeyTime="0"
																	Value="{StaticResource Color3}" />
										</ObjectAnimationUsingKeyFrames>
										<DoubleAnimation Storyboard.TargetName="DownArrowTransformation"
														 Storyboard.TargetProperty="Angle"
														 To="0"
														 Duration="0:0:0.3" />
									</Storyboard>
								</VisualState>
								<VisualState x:Name="Pressed">
									<Storyboard>
										<ObjectAnimationUsingKeyFrames Storyboard.TargetProperty="Foreground"
																	   Storyboard.TargetName="ContentPresenter">
											<DiscreteObjectKeyFrame KeyTime="0"
																	Value="{StaticResource Color3}" />
										</ObjectAnimationUsingKeyFrames>
										<PointerDownThemeAnimation Storyboard.TargetName="RootGrid" />
										<DoubleAnimation Storyboard.TargetName="DownArrowTransformation"
														 Storyboard.TargetProperty="Angle"
														 To="0"
														 Duration="0:0:0.3" />
									</Storyboard>
								</VisualState>
								<VisualState x:Name="Disabled">
									<Storyboard>
										<ObjectAnimationUsingKeyFrames Storyboard.TargetProperty="Foreground"
																	   Storyboard.TargetName="ContentPresenter">
											<DiscreteObjectKeyFrame KeyTime="0"
																	Value="{StaticResource Color2}" />
										</ObjectAnimationUsingKeyFrames>
										<DoubleAnimation Storyboard.TargetName="DownArrowTransformation"
														 Storyboard.TargetProperty="Angle"
														 To="0"
														 Duration="0:0:0.3" />
									</Storyboard>
								</VisualState>
								<VisualState x:Name="Checked">
									<Storyboard>
										<DoubleAnimation Storyboard.TargetName="DownArrowTransformation"
														 Storyboard.TargetProperty="Angle"
														 To="180"
														 Duration="0:0:0.3" />
									</Storyboard>
								</VisualState>
								<VisualState x:Name="CheckedPointerOver">
									<Storyboard>
										<DoubleAnimation Storyboard.TargetName="DownArrowTransformation"
														 Storyboard.TargetProperty="Angle"
														 To="180"
														 Duration="0:0:0.3" />
									</Storyboard>
								</VisualState>
								<VisualState x:Name="CheckedPressed">
									<Storyboard>
										<DoubleAnimation Storyboard.TargetName="DownArrowTransformation"
														 Storyboard.TargetProperty="Angle"
														 To="180"
														 Duration="0:0:0.3" />
									</Storyboard>
								</VisualState>
								<VisualState x:Name="CheckedDisabled">
									<Storyboard>
										<DoubleAnimation Storyboard.TargetName="DownArrowTransformation"
														 Storyboard.TargetProperty="Angle"
														 To="180"
														 Duration="0:0:0.3" />
									</Storyboard>
								</VisualState>
								<VisualState x:Name="Indeterminate" />
								<VisualState x:Name="IndeterminatePointerOver" />
								<VisualState x:Name="IndeterminatePressed" />
								<VisualState x:Name="IndeterminateDisabled" />
							</VisualStateGroup>
						</VisualStateManager.VisualStateGroups>
						<ContentPresenter x:Name="ContentPresenter"
										  AutomationProperties.AccessibilityView="Raw"
										  BorderBrush="{TemplateBinding BorderBrush}"
										  BorderThickness="{TemplateBinding BorderThickness}"
										  ContentTemplate="{TemplateBinding ContentTemplate}"
										  ContentTransitions="{TemplateBinding ContentTransitions}"
										  Content="{TemplateBinding Content}"
										  HorizontalContentAlignment="{TemplateBinding HorizontalContentAlignment}"
										  Padding="{TemplateBinding Padding}"
										  VerticalContentAlignment="{TemplateBinding VerticalContentAlignment}" />

						<Polygon x:Name="DropDownGlyph"
								 Points="0,0 8,0 4,4"
								 Fill="{StaticResource Color4}"
								 VerticalAlignment="Center"
								 HorizontalAlignment="Center"
								 RenderTransformOrigin="0.5,0.5">
							<Polygon.RenderTransform>
								<RotateTransform Angle="0"
												 x:Name="DownArrowTransformation" />
							</Polygon.RenderTransform>
						</Polygon>
					</Grid>
				</ControlTemplate>
			</Setter.Value>
		</Setter>
	</Style>
```

Second. you need to import this in your page.
```xml
xmlns:u="using:Umbrella.View.Controls"
```

## Properties

- **PreviewHeight** - The height of the control when in preview state.
- **PresentationHeight** - The height of the control when in presentation state.
- **IsOpened** - Set the default state or the control [preview/presentation].

## Usage

```xml
<u:ShowMore PreviewHeight="50"
            PresenterHeight="200"
            Style="{StaticResource DefautlShowMoreStyle}">
    <!-- Content of the ShowMore control -->
                [...]
</u:ShowMore>
````

## Knowed Issue

None.
