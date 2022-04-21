import React from 'react'
import {
	InputLabel,
	MenuItem,
	FormHelperText,
	FormControl,
	Select
} from '@material-ui/core'

function OnPageCountSwitcher(props) {
	return (
		<>
			<FormControl sx={{ m: 1, minWidth: 120 }}>
				<InputLabel id="simple-select-helper-label">Количество</InputLabel>
				<Select
					labelId="simple-select-helper-label"
					id="simple-select-helper"
					value={props.count}
					label="Количество"
					onChange={props.onChange}
				>
					<MenuItem value={3}>3</MenuItem>
					<MenuItem value={5}>5</MenuItem>
					<MenuItem value={10}>10</MenuItem>
					<MenuItem value={20}>20</MenuItem>
				</Select>
				<FormHelperText>на странице</FormHelperText>
			</FormControl>
		</>
	)
}

export default OnPageCountSwitcher