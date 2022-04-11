import { useState } from 'react'
import {
	createStyles,
	makeStyles,
	Button,
	Checkbox,
	List,
	ListItem,
	ListItemIcon,
	ListItemText,
} from '@material-ui/core'

const useStyles = makeStyles((theme) => createStyles({
	list: {
		height: theme.spacing(50),
		overflowY: "scroll"
	},
}));


export default function GenresSelector(props) {
	const classes = useStyles();
	const [checked, setChecked] = useState([...props.selected]);

	const handleToggle = (value) => () => {
		const currentIndex = checked.indexOf(value);
		const newChecked = [...checked];

		if (currentIndex === -1) {
			newChecked.push(value);
		} else {
			newChecked.splice(currentIndex, 1);
		}

		setChecked(newChecked);
		if (props.onChange && props.onChange !== null) {
			props.onChange(newChecked);
		}
	};


	return (
		<>
			<List className={classes.list}>
				{props.genres.map((genre) => {
					const labelId = `checkbox-list-label-${genre.value}`;

					return (
						<ListItem key={genre.value} disablePadding >
							<Button onClick={handleToggle(genre)} dense>
								<ListItemIcon>
									<Checkbox
										edge="start"
										checked={checked.indexOf(genre) !== -1}
										tabIndex={-1}
										disableRipple
										inputProps={{ 'aria-labelledby': labelId }}
									/>
								</ListItemIcon>
								<ListItemText id={labelId} primary={genre.value} />
							</Button>
						</ListItem>
					);
				})}
			</List>
		</>
	);
}
