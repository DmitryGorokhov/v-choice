import React from 'react'
import { Box, Link } from '@material-ui/core'
import Alert from '@material-ui/lab/Alert'

export default function MyAlerter(props) {
	return (
		<Box>
			{
				props.msg === null
					? props.error !== null
						? <Alert variant="outlined" severity="error">
							{props.error}<br />
							<Link href="/sign-in" variant="body2">
								Авторизироваться как администратор
								</Link>
						</Alert>
						: <Box></Box>
					: () => {
						return (
							<Alert variant="outlined" severity="success">
								{props.msg}<br />
								<Link href="/" variant="body2">
									Вернуться к фильмам
              								</Link>
							</Alert>
						)
					}
			}
		</Box>
	)
}
