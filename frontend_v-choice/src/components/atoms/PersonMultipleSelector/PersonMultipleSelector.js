import * as React from 'react'
import { Checkbox, FormControl, InputLabel, ListItemText, MenuItem, OutlinedInput, Select, } from '@material-ui/core'

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
	PaperProps: {
		style: {
			maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
			width: 250,
		},
	},
};

export default function PersonMultipleSelector(props) {
	const [selected, setSelected] = React.useState(props.selected);

	const handleChange = (event) => {
		const {
			target: { value },
		} = event;

		setSelected(value);
		props.onChange(value);
	};

	return (
		<>
			<FormControl>
				<InputLabel id="demo-multiple-checkbox-label">{props.label}</InputLabel>
				<Select
					labelId="demo-multiple-checkbox-label"
					id="demo-multiple-checkbox"
					multiple
					value={selected}
					onChange={handleChange}
					input={<OutlinedInput label={props.label} />}
					renderValue={(selected) => selected.map(item => item.fullName).join(', ')}
					MenuProps={MenuProps}
					className={props.className}
				>
					{props.list.map((person) => (
						<MenuItem key={person.Id} value={person}>
							<Checkbox checked={selected.indexOf(person) > -1} />
							<ListItemText primary={person.fullName} />
						</MenuItem>
					))}
				</Select>
			</FormControl>
		</>
	);
}
