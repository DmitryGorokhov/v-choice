import React, { useContext, useEffect, useState } from 'react'
import { createStyles, makeStyles, Box, Tab, Tabs, Typography } from '@material-ui/core'

import { NavMenu } from '../../atoms/NavMenu/NavMenu'
import GeneralStat from '../../moleculas/GeneralStat/GeneralStat';
import GenreStatisticTable from '../../moleculas/GenreStatisticTable/GenreStatisticTable';
import FilmStatisticTable from '../../moleculas/FilmStatisticTable/FilmStatisticTable';
import UserContext from '../../../context';


const useStyles = makeStyles((theme) => createStyles({
	container: {
		margin: theme.spacing(2),
		padding: theme.spacing(1),
	},
	header: {
		marginBottom: theme.spacing(2),
		textIndent: theme.spacing(1),
	}
}));

function TabPanel(props) {
	const classes = useStyles();
	const { children, value, index, ...other } = props;

	return (
		<div
			role="tabpanel"
			hidden={value !== index}
			id={`full-width-tabpanel-${index}`}
			aria-labelledby={`full-width-tab-${index}`}
			{...other}
		>
			{value === index && (
				<Box className={classes.container}>
					{children}
				</Box>
			)}
		</div>
	);
}

function a11yProps(index) {
	return {
		id: `full-width-tab-${index}`,
		'aria-controls': `full-width-tabpanel-${index}`,
	};
}

function Statistic() {
	const statURL = "https://localhost:5001/api/Statistic";
	const classes = useStyles();
	const { user, setUser } = useContext(UserContext);

	const [tab, setTab] = React.useState(0);
	const [general, setGeneral] = useState(null);
	const [loading, setLoading] = useState(true);

	const [filmStat, setFilmStat] = useState([]);
	const [filmStatTotal, setFilmStatTotal] = useState(0);
	const [filmStatQuery, setFilmStatQuery] = useState({
		page: 1, onPage: 10, filter: 0
	});

	const [genreStat, setGenreStat] = useState([]);
	const [genreStatTotal, setGenreStatTotal] = useState(0);
	const [genreStatQuery, setGenreStatQuery] = useState({
		page: 1, onPage: 10, filter: 0
	});

	const handleChangeTab = (event, newValue) => {
		setTab(newValue);
	};

	useEffect(() => {
		fetch(statURL)
			.then(response => response.json())
			.then(result => {
				setGeneral(result);
				setLoading(false);
			});
	}, [])

	useEffect(() => {
		fetch(`${statURL}/film` +
			`?SortingType=${filmStatQuery.filter}` +
			`&PageNumber=${filmStatQuery.page}` +
			`&OnPageCount=${filmStatQuery.onPage}`
		)
			.then(response => response.json())
			.then(result => {
				setFilmStat(result.items);
				setFilmStatTotal(result.totalCount);
				setLoading(false);
			})
			.catch(_ => _);
	}, [filmStatQuery])

	useEffect(() => {
		fetch(`${statURL}/genre` +
			`?SortingType=${genreStatQuery.filter}` +
			`&PageNumber=${genreStatQuery.page}` +
			`&OnPageCount=${genreStatQuery.onPage}`
		)
			.then(response => response.json())
			.then(result => {
				setGenreStat(result.items);
				setGenreStatTotal(result.totalCount);
				setLoading(false);
			})
			.catch(_ => _);
	}, [genreStatQuery])

	return (
		<>
			{
				user.isAdmin
					? <>
						<Tabs
							value={tab}
							onChange={handleChangeTab}
							textColor="inherit"
							indicatorColor="secondary"
							aria-label="tabs"
							centered
						>
							<Tab label="Общая статистика" {...a11yProps(0)} />
							<Tab label="Статистика по фильмам" {...a11yProps(1)} />
							<Tab label="Статистика по жанрам" {...a11yProps(2)} />
						</Tabs>

						<TabPanel value={tab} index={0} >
							<Typography variant='h4' className={classes.header}>Общая статистика</Typography>
							{
								loading
									? <Typography variant='body1'>Идет загрузка...</Typography>
									: <GeneralStat data={general} />
							}
						</TabPanel>
						<TabPanel value={tab} index={1}>
							<Typography variant='h4' className={classes.header}>Статистика по фильмам</Typography>
							<FilmStatisticTable
								loading={loading}
								setLoading={setLoading}
								data={filmStat}
								total={filmStatTotal}
								params={filmStatQuery}
								setParams={setFilmStatQuery}
							/>
						</TabPanel>
						<TabPanel value={tab} index={2}>
							<Typography variant='h4' className={classes.header}>Статистика по жанрам</Typography>
							<GenreStatisticTable
								loading={loading}
								setLoading={setLoading}
								data={genreStat}
								total={genreStatTotal}
								params={genreStatQuery}
								setParams={setGenreStatQuery}
							/>
						</TabPanel>
					</>
					: <Typography variant='subtitle1'>Для просмотра страницы необходимо войти как администратор</Typography>
			}
		</>
	)
}

export default Statistic