import React, { useState } from 'react'
import { createStyles, makeStyles, Box, Button, Collapse, Typography, } from '@material-ui/core'
import ExpandLessIcon from '@material-ui/icons/ExpandLess'
import ExpandMoreIcon from '@material-ui/icons/ExpandMore'

const useStyles = makeStyles((theme) => createStyles({
	container: {
		margin: theme.spacing(2, 0),
	},
	header: {
		margin: theme.spacing(2, 0),
		display: 'flex',
		justifyContent: 'space-between',
		alignItems: 'center',
	}
}));


function FilmVideo(props) {
	const classes = useStyles();
	const [open, setOpen] = useState(false);

	return (
		<>
			{
				props.videoToken
					? <Box className={classes.container}>
						<Box className={classes.header}>
							<Typography variant='h5'>Трейлер фильма на YouTube</Typography>
							<Button onClick={() => setOpen(!open)} size="small">
								{open ? <ExpandLessIcon fontSize="small" /> : <ExpandMoreIcon fontSize="small" />}
								{open ? "Свернуть" : "Развернуть"}
							</Button>
						</Box>
						<Collapse in={open} timeout="auto">
							<iframe
								width="560"
								height="315"
								src={`https://www.youtube.com/embed/${props.videoToken}`}
								title="YouTube video player"
								frameborder="0"
								allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
								allowfullscreen />
						</Collapse>
					</Box>
					: null
			}
		</>
	)
}

export default FilmVideo