import React from 'react'
import { Box } from '@material-ui/core'
import Alert from '@material-ui/lab/Alert'

export default function MyAlerter(props) {
	return (
		<Box>
			{
				props.msg === null
					? props.error !== null
						? <Alert variant="outlined" severity="error">
							{props.error}<br />
						</Alert>
						: <Box></Box>
					: () => {
						return (
							<Alert variant="outlined" severity="success">
								{props.msg}
							</Alert>
						)
					}
			}
		</Box>
	)
}
