# AnimatedContentControl

## Summary

The `AnimatedContentControl' apply an animation into a content with a style. 

## Platform support

| Feature                                                      | UWA | Android | iOS |
| ------------------------------------------------------------ |:---:|:-------:|:---:|
| Animate content                                              |  X  |    X    |  X  |

## Usage

Put the content you want to animate inside an animatedContent control like this:

```xaml
<u:AnimatedContentControl Style="{StaticResource AnimationStyle}"
							IsAnimating="{Binding [IsAnimatingAppearing]}">
    <!-- Content to animate -->
    [...]
</u:AnimatedContentControl>
```

You can create your own animation style like this.
```xaml
<Style x:Key="AnimationStyle"
		   TargetType="control:AnimatedContentControl">
	<Setter Property="Template">
		<Setter.Value>
			<ControlTemplate TargetType="control:AnimatedContentControl">
				<Grid>
					<VisualStateManager.VisualStateGroups>
						<VisualStateGroup x:Name="Animation">
							<VisualState x:Name="Animating">
								<Storyboard>
									<DoubleAnimation Storyboard.TargetName="Content"
														Storyboard.TargetProperty="Opacity"
														BeginTime="0:0:1"
														To="1"
														Duration="0:0:0.25" />
									<DoubleAnimation Storyboard.TargetName="Content"
														Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.Y)"
														BeginTime="0:0:1"
														Duration="0:0:0.25"
														To="0">
										<DoubleAnimation.EasingFunction>
											<CubicEase EasingMode="EaseOut" />
										</DoubleAnimation.EasingFunction>
									</DoubleAnimation>
								</Storyboard>
							</VisualState>
							<VisualState x:Name="NotAnimating">
								<Storyboard>
									<DoubleAnimation Storyboard.TargetName="Content"
														Storyboard.TargetProperty="Opacity"
														Duration="0:0:0"
														To="0" />
									<DoubleAnimation Storyboard.TargetName="Content"
														Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.Y)"
														Duration="0:0:0"
														To="150" />
								</Storyboard>
							</VisualState>
						</VisualStateGroup>
					</VisualStateManager.VisualStateGroups>

					<ContentPresenter Opacity="0"
										x:Name="Content"
										Content="{TemplateBinding Content}"
										ContentTemplate="{TemplateBinding ContentTemplate}"
										ContentTemplateSelector="{TemplateBinding ContentTemplateSelector}">
						<ContentPresenter.RenderTransform>
							<TranslateTransform Y="150" />
						</ContentPresenter.RenderTransform>
					</ContentPresenter>
				</Grid>
			</ControlTemplate>
		</Setter.Value>
	</Setter>
</Style>
```

## Known issues

None.
