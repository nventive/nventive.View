# PathControl

## Summary

`PathControl` allows for applying custom properties onto a Path for future reuse.

## Platform support

| Feature              | UWA | Android | iOS |
| ---------------------|:---:|:-------:|:---:|
| StoppedStoryboard    |  X  |    X    |  X  |
| LoadingStoryboard    |  X  |    X    |  X  |

## Usage

### Configure your page/control (XAML file).
- In the namespace section:
    ```xml
	xmlns:u="using:Umbrella.View.Controls"
    ```
- In the XAML:
    ```xml
		
		<u:PathControl Style="{StaticResource YourCustomPathControlStyle}"/>

    ```
- In the PathControl.xaml file:
    ```xml
    	<Style x:Key="YourCustomPathControlStyle"
			   TargetType="u:PathControl">
			<Setter Property="Stretch"
					Value="Fill" />
			<Setter Property="Template">
				<Setter.Value>
					<ControlTemplate TargetType="u:PathControl">
						<Path HorizontalAlignment="Center"
							  Data="1 5 10 e 25"/>
					</ControlTemplate>
				</Setter.Value>
			</Setter>
		</Style>
    ```
	
## Properties
'Stretch': How fitted the Path should be to the space it occupies
'Data': The Data Used to create the Path
'AnimationStopped': Boolean State to set or get state of animation //Doesn't actually change an ongoing animation.

## Known issues

None.
